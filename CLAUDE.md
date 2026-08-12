# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

Overtime (اورتایم) is a Persian-language, RTL sneaker/shoe e-commerce demo built with Blazor Server on .NET 10. The solution has three projects sharing one SQL Server database:

- **`src/Overtime.Data`** — EF Core `AppDbContext`, entities, migrations, and demo/seed data. Referenced by both apps; this is the only place schema changes happen.
- **`src/Overtime.Web`** — the public storefront (home, product listing/detail, cart, phone/OTP checkout, account).
- **`src/Overtime.Admin`** — the back-office (products, brands, orders, customers, admin user management).

Both apps are independent ASP.NET Core hosts (no shared process), each with their own `Program.cs`, auth cookie, and port, but they read/write the same `Overtime` database via `Overtime.Data`.

## Commands

Run from the repo root (`Overtime.slnx`).

```bash
dotnet build
dotnet run --project src/Overtime.Web    # storefront: http://localhost:5036
dotnet run --project src/Overtime.Admin  # back-office: http://localhost:5160
```

There is no test project in the solution and no CI workflow configured — don't assume `dotnet test` does anything.

EF Core migrations live in `src/Overtime.Data/Migrations` and target `AppDbContext`. Add a migration from the repo root with the startup project set to `Overtime.Web` (or `Overtime.Admin` — both share the same DbContext/connection string), e.g.:

```bash
dotnet ef migrations add <Name> --project src/Overtime.Data --startup-project src/Overtime.Web
```

Both `Overtime.Web` and `Overtime.Admin` call `db.Database.Migrate()` and seed demo data on every startup (see `Program.cs` in each), so pending migrations are applied automatically the next time either app runs — no manual `dotnet ef database update` needed in normal dev flow.

The default connection string (`appsettings.json` in both apps) points at `(localdb)\mssqllocaldb`, database `Overtime`.

## Architecture notes

### Auth is per-app and different per audience

- **Web**: customers authenticate via phone number + OTP (`OtpService`), not passwords. No SMS gateway is wired up — in dev, the OTP code is only written to the log (`logger.LogInformation`). Cookie scheme name `overtime_auth`, 30-day sliding expiration.
- **Admin**: username/password login. The first admin account is bootstrapped from config (`Admin:Username` / `Admin:Password` / `Admin:OwnerName` in `appsettings.json`) the first time `AdminUsers` is empty (`AdminBootstrapper`); after that, admins are managed from the "مدیریت ادمین‌ها" page. Cookie scheme `overtime_admin_auth`, 12-hour expiration, and `Program.cs` sets a global fallback policy requiring an authenticated user for every endpoint (unlike Web, which is anonymous by default).

### Cart is anonymous-first

Every visitor to Web gets a `Guid` cart id in an HttpOnly cookie (set in `Program.cs` middleware, see `CartCookie`), independent of login. `CartContext`/`CartService` key off that cookie id, so carts work before a customer ever authenticates and persist across the OTP login.

### Payments are scaffolded, not wired up

`src/Overtime.Web/Services/Payments/` (`IPaymentGateway`, `ManualPaymentGateway`, plus `RequestRateLimiter`) and the `Payment`/`PaymentMethod`/`PaymentStatus` entities exist but are **not** registered in `Program.cs` DI and not referenced by `OrderService` or any page yet. Treat this as in-progress infrastructure, not a working checkout path, until it's actually wired in.

### No frontend framework — hand-rolled CSS design system

Neither app references a Blazor component library (MudBlazor was tried for a carousel and reverted — see git history around "Revert MudCarousel"). All UI is plain Razor + hand-written CSS. Key conventions:

- Global theming via CSS custom properties on `:root`, redefined under `@media (prefers-color-scheme: dark)` and `:root[data-theme="dark"]`/`:root[data-theme="light"]` for an explicit user toggle. Theme state is read/written through `window.overtimeTheme` in `wwwroot/js/interop.js` and mirrored to `localStorage`.
- Everything is RTL (`direction: rtl` on `html`/`body`, logical CSS properties like `inset-inline`/`border-inline-end` preferred over `left`/`right` for anything that needs to work correctly in RTL).
- Persian digits/formatting go through `PersianNumberFormatter` (present in both `Overtime.Web/Services` and `Overtime.Admin/Services` — not shared, so keep them in sync if you change one).
- Icons are inline SVG paths in a single `Icon.razor` component per app (`Name` string switched to a `<path>`/`<line>` fragment) — not separate `.svg` files. Brand logos, by contrast, *are* real `.svg` files under `wwwroot/assets/images/brands/`.

### wwwroot CSS files are all loaded globally — watch for cross-file drift

`Overtime.Web`'s `wwwroot/assets/css/` has one stylesheet per page area (`index.css`, `products.css`, `product.css`, `account.css`), but **all of them are linked on every page**, not scoped per-route. Several of these files contain near-duplicate copies of shared chrome rules (header/nav/mega-menu), which have drifted from each other before (e.g. `product.css` once shadowed `index.css`'s mobile nav-accordion height because it loads later in `<head>` and had an equal-specificity selector with a different value). When editing shared header/nav/footer CSS, grep across all four files for the selector you're touching, not just the one that looks most relevant.

### Visual design language — "Soft Modern Commerce"

This is the established look of the storefront (`index.css` `:root`) and should be followed for any new UI rather than improvised per-component:

- **Color**: a single purple accent (`--accent: #6c2fd6` light / `#8355f2` dark) used sparingly — CTAs, active/hover states, highlights — against neutral `--bg`/`--surface`/`--surface-2` and `--ink`/`--ink-soft`/`--muted` text tones. Every color is a CSS custom property; never hardcode a hex value in a rule, redefine the token instead (see the three `:root` blocks in `index.css` for the light/dark/explicit-toggle pattern).
- **Shape**: `--radius: 18px` for panels/cards/dropdowns; fully pill-shaped (`border-radius: 999px`) for buttons and badges. Sharp corners don't appear anywhere in the existing UI.
- **Elevation**: `--shadow` (soft, diffused, large blur/low-opacity) is reserved for things that float above content — dropdowns, drawers, the hero art, floating cart — not applied to plain in-flow cards, which use a 1px `--line` border instead.
- **Motion**: purposeful, not decorative — a hover effect should exist because it clarifies something is interactive or floating, not by default on every element. Use `cubic-bezier(0.16, 1, 0.3, 1)` ("ease-out-expo") for anything entering/settling (panels, drawers, dropdowns) instead of linear `ease` — it decelerates hard at the end, which reads as deliberate rather than mechanical. Scale duration to distance/size: ~120ms for color/opacity-only changes, ~200ms for small transforms, ~300–360ms for full panels (mega-menu, drawer). Reserve `translateY` lift for primary, emphasis-worthy cards (product cards, not every link or logo); for smaller elements prefer a subtler cue (border/background color shift, or a small `scale(0.97)` on `:active` for tactile press feedback) instead of reaching for the same lift everywhere. Every animation still needs a `@media (prefers-reduced-motion: reduce)` fallback that disables it.
- **Typography**: Vazirmatn (variable font, weights 100–900), bold (700–900) for headings/CTAs, `--muted` for de-emphasized text, 1.7 body line-height.
- **Rhythm**: `.wrap` caps content at 1180px with 24px side gutters; sections use 88px vertical padding (56px at the ≤560px breakpoint).
- **Chrome**: the sticky header's frosted-glass treatment (`backdrop-filter: blur(10px)` over a translucent `--bg`) is the site's recurring "floating over content" motif — reuse it for any new overlay that should feel anchored to the chrome rather than opaque.

### Solution/project layout

- `Overtime.slnx` (the new XML-based slnx format, not `.sln`) lists the three projects under a `/src/` folder.
- `.claude/launch.json` defines the two dev-server preview configs (`overtime-web` on 5036, `overtime-admin` on 5160) used by the Browser-pane preview tooling.
- Target framework is `net10.0` for all three projects, with `Nullable` and `ImplicitUsings` enabled. Primary constructors are used throughout services (e.g. `BrandService(IDbContextFactory<AppDbContext> dbFactory)`), and DTOs for things like payment results are C# `record`s.
