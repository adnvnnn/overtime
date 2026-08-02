(function(){
  "use strict";
  var STORAGE_KEY = "overtime_cart_v1";
  var SWATCH_VAR = { accent: "--accent", "accent-2": "--accent-2", ink: "--ink" };
  var cart = loadCart();
  var successMode = false;

  var elToggle = document.getElementById("cartToggle");
  var elBackdrop = document.getElementById("cartBackdrop");
  var elDrawer = document.getElementById("cartDrawer");
  var elClose = document.getElementById("cartClose");
  var elBody = document.getElementById("cartBody");
  var elFoot = document.getElementById("cartFoot");
  var elCount = document.getElementById("cartCount");
  var elSubtotal = document.getElementById("cartSubtotal");
  var elCheckout = document.getElementById("cartCheckout");
  var elToast = document.getElementById("toast");
  var toastTimer = null;

  function loadCart(){
    try {
      var raw = localStorage.getItem(STORAGE_KEY);
      return raw ? JSON.parse(raw) : {};
    } catch (e) { return {}; }
  }
  function saveCart(){
    try { localStorage.setItem(STORAGE_KEY, JSON.stringify(cart)); } catch (e) {}
  }
  function toFa(input){
    var fa = ["۰","۱","۲","۳","۴","۵","۶","۷","۸","۹"];
    return String(input).replace(/[0-9]/g, function(d){ return fa[d]; });
  }
  function formatToman(n){
    return toFa(Math.round(n).toLocaleString("en-US")) + " تومان";
  }
  function swatchStyle(key){
    var v = SWATCH_VAR[key] || SWATCH_VAR.accent;
    return "background:color-mix(in srgb, var(" + v + ") 16%, var(--surface-2)); color:var(" + v + ")";
  }
  function cartEntries(){ return Object.keys(cart).map(function(id){ return Object.assign({ id: id }, cart[id]); }); }
  function cartCount(){ return cartEntries().reduce(function(sum, it){ return sum + it.qty; }, 0); }
  function cartTotal(){ return cartEntries().reduce(function(sum, it){ return sum + it.qty * it.price; }, 0); }

  function updateBadge(){
    var count = cartCount();
    elCount.textContent = toFa(count);
    elCount.setAttribute("data-empty", count === 0 ? "true" : "false");
  }
  function bumpBadge(){
    elCount.classList.remove("bump");
    void elCount.offsetWidth;
    elCount.classList.add("bump");
  }

  function renderCart(){
    updateBadge();
    var items = cartEntries();
    if (items.length === 0){
      elBody.innerHTML =
        '<div class="cart-empty">' +
          '<i data-lucide="shopping-cart"></i>' +
          '<span>سبد خرید شما خالی است</span>' +
        '</div>';
      elCheckout.disabled = true;
      elSubtotal.textContent = formatToman(0);
      if (window.lucide) lucide.createIcons();
      return;
    }
    elCheckout.disabled = false;
    elBody.innerHTML = items.map(function(it){
      return (
        '<div class="cart-item" data-row="' + it.id + '">' +
          (it.image
            ? '<div class="cart-item-swatch"><img src="' + it.image + '" alt="' + it.name + '"></div>'
            : '<div class="cart-item-swatch" style="' + swatchStyle(it.swatch) + '"><i data-lucide="footprints" class="mini-shoe"></i></div>'
          ) +
          '<div class="cart-item-info">' +
            '<b>' + it.name + '</b>' +
            '<span>' + it.cat + '</span>' +
            '<div class="qty-stepper">' +
              '<button type="button" data-dec aria-label="کاهش تعداد">−</button>' +
              '<span>' + toFa(it.qty) + '</span>' +
              '<button type="button" data-inc aria-label="افزایش تعداد">+</button>' +
            '</div>' +
          '</div>' +
          '<div class="cart-item-end">' +
            '<button class="cart-remove" type="button" data-remove aria-label="حذف از سبد">حذف</button>' +
            '<b class="cart-item-price">' + formatToman(it.qty * it.price) + '</b>' +
          '</div>' +
        '</div>'
      );
    }).join("");
    elSubtotal.textContent = formatToman(cartTotal());
    if (window.lucide) lucide.createIcons();
  }

  function addToCart(btn){
    var card = btn.closest(".card");
    if (!card) return;
    var id = card.dataset.id;
    var price = parseInt(card.dataset.price, 10) || 0;
    if (cart[id]) {
      cart[id].qty += 1;
    } else {
      cart[id] = {
        name: card.dataset.name,
        cat: card.dataset.cat,
        price: price,
        swatch: card.dataset.swatch || "accent",
        image: card.dataset.image || "",
        qty: 1
      };
    }
    saveCart();
    renderCart();
    bumpBadge();
    showToast(card.dataset.name + " به سبد اضافه شد");

    var original = btn.textContent;
    btn.textContent = "افزوده شد ✓";
    btn.disabled = true;
    setTimeout(function(){ btn.textContent = original; btn.disabled = false; }, 1100);
  }

  function showToast(msg){
    elToast.textContent = msg;
    elToast.classList.add("show");
    clearTimeout(toastTimer);
    toastTimer = setTimeout(function(){ elToast.classList.remove("show"); }, 2200);
  }

  function openDrawer(){
    successMode = false;
    renderCart();
    elBackdrop.classList.add("open");
    elDrawer.classList.add("open");
    elDrawer.setAttribute("aria-hidden", "false");
    document.body.style.overflow = "hidden";
    elClose.focus();
  }
  function closeDrawer(){
    elBackdrop.classList.remove("open");
    elDrawer.classList.remove("open");
    elDrawer.setAttribute("aria-hidden", "true");
    document.body.style.overflow = "";
    elToggle.focus();
    if (successMode){ successMode = false; renderCart(); }
  }

  function checkout(){
    if (cartCount() === 0) return;
    var orderNo = "OT-" + Math.floor(10000 + Math.random() * 89999);
    successMode = true;
    elBody.innerHTML =
      '<div class="cart-success">' +
        '<div class="ok">✓</div>' +
        '<h4>سفارش شما ثبت شد</h4>' +
        '<p>این یک فروشگاه نمایشی است؛ همکاران اورتایم به‌زودی برای هماهنگی ارسال با شما تماس می‌گیرند.</p>' +
        '<span class="ordno">شماره سفارش دمو: ' + toFa(orderNo) + '</span>' +
        '<button class="btn btn-ghost btn-sm" type="button" data-success-close>بستن</button>' +
      '</div>';
    cart = {};
    saveCart();
    updateBadge();
  }

  elToggle.addEventListener("click", openDrawer);
  elClose.addEventListener("click", closeDrawer);
  elBackdrop.addEventListener("click", closeDrawer);
  document.addEventListener("keydown", function(e){
    if (e.key === "Escape" && elDrawer.classList.contains("open")) closeDrawer();
  });
  elCheckout.addEventListener("click", checkout);

  document.querySelectorAll("[data-add]").forEach(function(btn){
    btn.addEventListener("click", function(){ addToCart(btn); });
  });

  elBody.addEventListener("click", function(e){
    var incBtn = e.target.closest("[data-inc]");
    var decBtn = e.target.closest("[data-dec]");
    var rmBtn = e.target.closest("[data-remove]");
    var successCloseBtn = e.target.closest("[data-success-close]");
    if (successCloseBtn){ closeDrawer(); return; }
    var row = e.target.closest(".cart-item");
    if (!row) return;
    var id = row.dataset.row;
    if (!cart[id]) return;
    if (incBtn){ cart[id].qty += 1; }
    else if (decBtn){ cart[id].qty -= 1; if (cart[id].qty <= 0) delete cart[id]; }
    else if (rmBtn){ delete cart[id]; }
    else { return; }
    saveCart();
    renderCart();
  });

  updateBadge();

  var themeToggle = document.getElementById("themeToggle");
  function currentTheme(){
    return document.documentElement.getAttribute("data-theme") === "dark" ? "dark" : "light";
  }
  function setTheme(theme){
    document.documentElement.setAttribute("data-theme", theme);
    try { localStorage.setItem("overtime_theme", theme); } catch (e) {}
    themeToggle.setAttribute("data-current", theme);
  }
  themeToggle.setAttribute("data-current", currentTheme());
  themeToggle.addEventListener("click", function(){
    setTheme(currentTheme() === "dark" ? "light" : "dark");
  });

  var menuToggle = document.getElementById("menuToggle");
  var navLinks = document.getElementById("navLinks");

  function openMenu(){
    navLinks.classList.add("open");
    menuToggle.setAttribute("aria-expanded", "true");
  }
  function closeMenu(){
    navLinks.classList.remove("open");
    menuToggle.setAttribute("aria-expanded", "false");
  }
  menuToggle.addEventListener("click", function(){
    if (navLinks.classList.contains("open")) closeMenu(); else openMenu();
  });
  navLinks.addEventListener("click", function(e){
    if (e.target.closest("a")) closeMenu();
  });
  document.addEventListener("click", function(e){
    if (!navLinks.classList.contains("open")) return;
    if (e.target.closest("#navLinks") || e.target.closest("#menuToggle")) return;
    closeMenu();
  });
  document.addEventListener("keydown", function(e){
    if (e.key === "Escape") closeMenu();
  });
  window.addEventListener("resize", function(){
    if (window.innerWidth > 900) closeMenu();
  });
})();
