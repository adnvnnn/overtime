(function(){
  "use strict";
  var STORAGE_KEY = "overtime_cart_v1";
  var SWATCH_VAR = { accent: "--accent", "accent-2": "--accent-2", ink: "--ink" };

  var PRODUCTS = {
    "air-pulse": {
      name: "Air Pulse", cat: "مردانه · رانینگ", price: 2950000, swatch: "accent", badge: "پرفروش",
      image: "assets/images/products/product-1.jpg"
    },
    "highjump-x": {
      name: "Highjump X", cat: "مردانه · بسکتبال", price: 3800000, swatch: "accent-2", badge: "جدید",
      image: "assets/images/products/product-2.jpg"
    },
    "street-low": {
      name: "Street Low", cat: "یونیسکس · روزمره", price: 2200000, swatch: "ink", badge: null,
      image: "assets/images/products/product-3.jpg"
    },
    "aria-knit": {
      name: "Aria Knit", cat: "زنانه · نیت", price: 2650000, swatch: "accent-2", badge: "تخفیف",
      image: "assets/images/products/product-4.jpg"
    },
    "mini-dash": {
      name: "Mini Dash", cat: "بچگانه · روزمره", price: 1850000, swatch: "accent", badge: null,
      image: "assets/images/products/product-5.jpg"
    },
    "overtime-06": {
      name: "Overtime №06", cat: "لیمیتد ادیشن", price: 4500000, swatch: "ink", badge: "لیمیتد",
      image: "assets/images/products/product-6.jpg"
    }
  };

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

  /* ---------- catalog: filter, search, sort, render ---------- */
  var allIds = Object.keys(PRODUCTS);
  var state = { cat: "all", query: "", sort: "default" };

  var grid = document.getElementById("catalogGrid");
  var emptyEl = document.getElementById("catalogEmpty");
  var countEl = document.getElementById("catalogCount");
  var searchInput = document.getElementById("catalogSearch");
  var sortSelect = document.getElementById("catalogSort");
  var filterTabs = document.getElementById("filterTabs");

  function cardHtml(id){
    var p = PRODUCTS[id];
    var tagHtml = p.badge ? '<span class="tag">' + p.badge + '</span>' : '';
    return (
      '<article class="card" data-id="' + id + '" data-name="' + p.name + '" data-cat="' + p.cat + '" data-price="' + p.price + '" data-swatch="' + p.swatch + '" data-image="' + p.image + '">' +
        '<a class="card-link" href="product.html?id=' + id + '" aria-label="مشاهده جزئیات ' + p.name + '">' +
          '<div class="plate" style="background:color-mix(in srgb, var(' + SWATCH_VAR[p.swatch] + ') 14%, var(--surface-2));">' +
            tagHtml +
            '<img src="' + p.image + '" alt="' + p.name + '" loading="lazy">' +
          '</div>' +
        '</a>' +
        '<div class="body">' +
          '<span class="cat-line">' + p.cat + '</span>' +
          '<h3><a href="product.html?id=' + id + '">' + p.name + '</a></h3>' +
          '<div class="row">' +
            '<span class="price">' + toFa(p.price.toLocaleString("en-US")) + '<small>تومان</small></span>' +
            '<button class="add" type="button" data-add>افزودن به سبد</button>' +
          '</div>' +
        '</div>' +
      '</article>'
    );
  }

  function matchesCat(id, cat){
    return cat === "all" || PRODUCTS[id].cat.indexOf(cat) !== -1;
  }
  function matchesQuery(id, query){
    if (!query) return true;
    var p = PRODUCTS[id];
    return (p.name + " " + p.cat).toLowerCase().indexOf(query) !== -1;
  }
  function sortIds(ids){
    var sorted = ids.slice();
    if (state.sort === "price-asc"){
      sorted.sort(function(a, b){ return PRODUCTS[a].price - PRODUCTS[b].price; });
    } else if (state.sort === "price-desc"){
      sorted.sort(function(a, b){ return PRODUCTS[b].price - PRODUCTS[a].price; });
    } else if (state.sort === "name"){
      sorted.sort(function(a, b){ return PRODUCTS[a].name.localeCompare(PRODUCTS[b].name); });
    }
    return sorted;
  }

  function renderCatalog(){
    var query = state.query.trim().toLowerCase();
    var ids = allIds.filter(function(id){ return matchesCat(id, state.cat) && matchesQuery(id, query); });
    ids = sortIds(ids);

    countEl.textContent = toFa(ids.length) + " از " + toFa(allIds.length) + " محصول";

    if (ids.length === 0){
      grid.innerHTML = "";
      emptyEl.hidden = false;
    } else {
      emptyEl.hidden = true;
      grid.innerHTML = ids.map(cardHtml).join("");
    }
  }

  searchInput.addEventListener("input", function(){
    state.query = searchInput.value;
    renderCatalog();
  });
  sortSelect.addEventListener("change", function(){
    state.sort = sortSelect.value;
    renderCatalog();
  });
  filterTabs.addEventListener("click", function(e){
    var tab = e.target.closest(".filter-tab");
    if (!tab) return;
    filterTabs.querySelectorAll(".filter-tab").forEach(function(t){ t.classList.remove("active"); });
    tab.classList.add("active");
    state.cat = tab.dataset.cat;
    renderCatalog();
  });
  document.getElementById("catalogReset").addEventListener("click", function(){
    state.cat = "all"; state.query = ""; state.sort = "default";
    searchInput.value = ""; sortSelect.value = "default";
    filterTabs.querySelectorAll(".filter-tab").forEach(function(t){ t.classList.remove("active"); });
    filterTabs.querySelector('[data-cat="all"]').classList.add("active");
    renderCatalog();
  });

  var catParam = new URLSearchParams(window.location.search).get("cat");
  if (catParam) {
    var preTab = filterTabs.querySelector('[data-cat="' + CSS.escape(catParam) + '"]');
    if (preTab) {
      filterTabs.querySelectorAll(".filter-tab").forEach(function(t){ t.classList.remove("active"); });
      preTab.classList.add("active");
      state.cat = catParam;
    }
  }
  renderCatalog();

  /* ---------- cart ---------- */
  var cart = loadCart();
  var successMode = false;

  var elToggle = document.getElementById("cartToggle");
  var elBackdrop = document.getElementById("cartBackdrop");
  var elDrawer = document.getElementById("cartDrawer");
  var elClose = document.getElementById("cartClose");
  var elBody = document.getElementById("cartBody");
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

  function addToCart(id, name, cat, price, swatch, qty, image){
    if (cart[id]) {
      cart[id].qty += qty;
    } else {
      cart[id] = { name: name, cat: cat, price: price, swatch: swatch || "accent", image: image || "", qty: qty };
    }
    saveCart();
    renderCart();
    bumpBadge();
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

  document.addEventListener("click", function(e){
    var addBtn = e.target.closest("[data-add]");
    if (!addBtn) return;
    var card = addBtn.closest(".card");
    if (!card) return;
    var cid = card.dataset.id;
    addToCart(cid, card.dataset.name, card.dataset.cat, parseInt(card.dataset.price, 10) || 0, card.dataset.swatch, 1, card.dataset.image);
    showToast(card.dataset.name + " به سبد اضافه شد");
    var original = addBtn.textContent;
    addBtn.textContent = "افزوده شد ✓";
    addBtn.disabled = true;
    setTimeout(function(){ addBtn.textContent = original; addBtn.disabled = false; }, 1100);
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
