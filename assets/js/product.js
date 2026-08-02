(function(){
  "use strict";
  var STORAGE_KEY = "overtime_cart_v1";
  var SWATCH_VAR = { accent: "--accent", "accent-2": "--accent-2", ink: "--ink" };

  var PRODUCTS = {
    "air-pulse": {
      name: "Air Pulse", cat: "مردانه · رانینگ", price: 2950000, swatch: "accent", badge: "پرفروش",
      image: "assets/images/products/product-1.jpg",
      images: ["assets/images/products/product-1.jpg", "assets/images/products/product-1.jpg", "assets/images/products/product-1.jpg"],
      rating: 4.8, reviews: 124,
      desc: "کتانی رانینگ Air Pulse برای دویدن‌های روزمره و تمرین‌های سبک طراحی شده است. رویه‌ی مش تنفس‌پذیر همراه با میان‌کفش سبک، احساس راحتی را در هر قدم حفظ می‌کند.",
      specs: { "جنس رویه":"مش تنفس‌پذیر", "نوع بند":"بندی معمولی", "کاربرد":"دویدن و پیاده‌روی", "رنگ اصلی":"بنفش" }
    },
    "highjump-x": {
      name: "Highjump X", cat: "مردانه · بسکتبال", price: 3800000, swatch: "accent-2", badge: "جدید",
      image: "assets/images/products/product-2.jpg",
      images: ["assets/images/products/product-2.jpg", "assets/images/products/product-2.jpg", "assets/images/products/product-2.jpg"],
      rating: 4.7, reviews: 98,
      desc: "Highjump X با میان‌کفش ضربه‌گیر و ساق بلندتر، برای حرکات ناگهانی در زمین بسکتبال طراحی شده و پایداری مچ پا را در فرودها افزایش می‌دهد.",
      specs: { "جنس رویه":"سینتتیک و مش", "نوع بند":"بندی بلند", "کاربرد":"بسکتبال", "رنگ اصلی":"بنفش تیره" }
    },
    "street-low": {
      name: "Street Low", cat: "یونیسکس · روزمره", price: 2200000, swatch: "ink", badge: null,
      image: "assets/images/products/product-3.jpg",
      images: ["assets/images/products/product-3.jpg", "assets/images/products/product-3.jpg", "assets/images/products/product-3.jpg"],
      rating: 4.6, reviews: 156,
      desc: "یک کتانی روزمره‌ی ساده و همه‌کاره؛ Street Low با طراحی مینیمال و رنگ خنثی، با هر پوششی هم‌نشین می‌شود.",
      specs: { "جنس رویه":"چرم مصنوعی", "نوع بند":"بندی معمولی", "کاربرد":"استفاده روزمره", "رنگ اصلی":"مشکی" }
    },
    "aria-knit": {
      name: "Aria Knit", cat: "زنانه · نیت", price: 2650000, swatch: "accent-2", badge: "تخفیف",
      image: "assets/images/products/product-4.jpg",
      images: ["assets/images/products/product-4.jpg", "assets/images/products/product-4.jpg", "assets/images/products/product-4.jpg"],
      rating: 4.8, reviews: 142,
      desc: "Aria Knit با رویه‌ی نیت کشباف، فرمی نزدیک به فرم پا دارد و برای پوشیدن طولانی‌مدت در طول روز بسیار راحت است.",
      specs: { "جنس رویه":"نیت کشباف", "نوع بند":"بندی کشی", "کاربرد":"استفاده روزمره", "رنگ اصلی":"بنفش تیره" }
    },
    "mini-dash": {
      name: "Mini Dash", cat: "بچگانه · روزمره", price: 1850000, swatch: "accent", badge: null,
      image: "assets/images/products/product-5.jpg",
      images: ["assets/images/products/product-5.jpg", "assets/images/products/product-5.jpg", "assets/images/products/product-5.jpg"],
      rating: 4.9, reviews: 86,
      desc: "Mini Dash مدلی سبک و بادوام برای کودکان است؛ زیره‌ی ضدلغزش و بست چسبی، پوشیدن و درآوردن را برای کودکان آسان می‌کند.",
      specs: { "جنس رویه":"مش نرم", "نوع بند":"بست چسبی", "کاربرد":"استفاده روزمره", "رنگ اصلی":"بنفش" }
    },
    "overtime-06": {
      name: "Overtime №06", cat: "لیمیتد ادیشن", price: 4500000, swatch: "ink", badge: "لیمیتد",
      image: "assets/images/products/product-6.jpg",
      images: ["assets/images/products/product-6.jpg", "assets/images/products/product-6.jpg", "assets/images/products/product-6.jpg"],
      rating: 5.0, reviews: 41,
      desc: "Overtime №06 نسخه‌ی محدود امضای برند اورتایم است؛ تیراژ کم، جزئیات ویژه و جعبه‌ی اختصاصی این مدل را به یک قطعه‌ی کالکشنی تبدیل می‌کند.",
      specs: { "جنس رویه":"چرم و سوئد", "نوع بند":"بندی معمولی", "کاربرد":"کالکشنی و روزمره", "رنگ اصلی":"مشکی و بنفش" }
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

  var params = new URLSearchParams(window.location.search);
  var pid = params.get("id");
  if (!PRODUCTS[pid]) pid = "air-pulse";
  var product = PRODUCTS[pid];

  document.title = "Overtime | " + product.name;
  document.getElementById("crumbCurrent").textContent = product.name;

  var gallery = document.getElementById("pdpGallery");
  gallery.style.background = "color-mix(in srgb, var(" + SWATCH_VAR[product.swatch] + ") 14%, var(--surface-2))";
  var tagEl = document.getElementById("pdpTag");
  if (product.badge) { tagEl.textContent = product.badge; } else { tagEl.style.display = "none"; }

  /* ---------- image slider ---------- */
  var galleryImages = product.images && product.images.length ? product.images : [product.image];
  var sliderEl = document.getElementById("pdpSlider");
  var thumbsEl = document.getElementById("pdpThumbs");
  var dotsEl = document.getElementById("pdpDots");
  var prevBtn = document.getElementById("pdpPrev");
  var nextBtn = document.getElementById("pdpNext");
  var activeIndex = 0;
  var multi = galleryImages.length > 1;

  sliderEl.innerHTML = galleryImages.map(function(src){
    return '<div class="pdp-slide"><img src="' + src + '" alt="' + product.name + '" draggable="false"></div>';
  }).join("");

  if (multi) {
    thumbsEl.innerHTML = galleryImages.map(function(src, i){
      return '<button type="button" class="pdp-thumb" data-idx="' + i + '" aria-label="تصویر ' + toFa(i + 1) + '"><img src="' + src + '" alt="' + product.name + '"></button>';
    }).join("");
    dotsEl.innerHTML = galleryImages.map(function(_, i){
      return '<button type="button" class="pdp-dot" data-idx="' + i + '" aria-label="تصویر ' + toFa(i + 1) + '"></button>';
    }).join("");
  } else {
    thumbsEl.innerHTML = "";
    dotsEl.innerHTML = "";
    prevBtn.hidden = true;
    nextBtn.hidden = true;
  }

  function goTo(idx){
    activeIndex = ((idx % galleryImages.length) + galleryImages.length) % galleryImages.length;
    sliderEl.style.transform = "translateX(-" + (activeIndex * 100) + "%)";
    thumbsEl.querySelectorAll(".pdp-thumb").forEach(function(t, i){ t.classList.toggle("active", i === activeIndex); });
    dotsEl.querySelectorAll(".pdp-dot").forEach(function(d, i){ d.classList.toggle("active", i === activeIndex); });
  }
  goTo(0);

  if (multi) {
    thumbsEl.addEventListener("click", function(e){
      var btn = e.target.closest(".pdp-thumb");
      if (!btn) return;
      goTo(parseInt(btn.dataset.idx, 10));
    });
    dotsEl.addEventListener("click", function(e){
      var btn = e.target.closest(".pdp-dot");
      if (!btn) return;
      goTo(parseInt(btn.dataset.idx, 10));
    });
    prevBtn.addEventListener("click", function(){ goTo(activeIndex - 1); });
    nextBtn.addEventListener("click", function(){ goTo(activeIndex + 1); });

    /* ---------- swipe / drag ---------- */
    var dragging = false, dragStartX = 0, dragDeltaX = 0;
    function pointerX(e){ return e.touches ? e.touches[0].clientX : e.clientX; }
    function dragStart(e){
      dragging = true; dragDeltaX = 0;
      dragStartX = pointerX(e);
      sliderEl.classList.add("dragging");
    }
    function dragMove(e){
      if (!dragging) return;
      dragDeltaX = pointerX(e) - dragStartX;
      var basePercent = -activeIndex * 100;
      var deltaPercent = (dragDeltaX / sliderEl.clientWidth) * 100;
      sliderEl.style.transform = "translateX(" + (basePercent + deltaPercent) + "%)";
    }
    function dragEnd(){
      if (!dragging) return;
      dragging = false;
      sliderEl.classList.remove("dragging");
      var threshold = sliderEl.clientWidth * 0.16;
      if (dragDeltaX < -threshold) { goTo(activeIndex + 1); }
      else if (dragDeltaX > threshold) { goTo(activeIndex - 1); }
      else { goTo(activeIndex); }
    }
    sliderEl.addEventListener("touchstart", dragStart, { passive: true });
    sliderEl.addEventListener("touchmove", dragMove, { passive: true });
    sliderEl.addEventListener("touchend", dragEnd);
    sliderEl.addEventListener("mousedown", function(e){ e.preventDefault(); dragStart(e); });
    window.addEventListener("mousemove", dragMove);
    window.addEventListener("mouseup", dragEnd);
  }

  document.getElementById("pdpCat").textContent = product.cat;
  document.getElementById("pdpTitle").textContent = product.name;
  document.getElementById("pdpRatingNum").textContent = toFa(product.rating.toFixed(1));
  document.getElementById("pdpReviews").textContent = "(" + toFa(product.reviews) + " نظر)";
  document.getElementById("pdpPrice").innerHTML = toFa(product.price.toLocaleString("en-US")) + "<small>تومان</small>";
  document.getElementById("pdpDesc").textContent = product.desc;

  document.getElementById("pdpSpecs").innerHTML = Object.keys(product.specs).map(function(k){
    return "<div><span>" + k + "</span><b>" + product.specs[k] + "</b></div>";
  }).join("");

  var relatedIds = Object.keys(PRODUCTS).filter(function(k){ return k !== pid; }).slice(0, 3);
  document.getElementById("relatedGrid").innerHTML = relatedIds.map(function(rid){
    var p = PRODUCTS[rid];
    var tagHtml = p.badge ? '<span class="tag">' + p.badge + '</span>' : '';
    return (
      '<article class="card" data-id="' + rid + '" data-name="' + p.name + '" data-cat="' + p.cat + '" data-price="' + p.price + '" data-swatch="' + p.swatch + '" data-image="' + p.image + '">' +
        '<a class="card-link" href="product.html?id=' + rid + '" aria-label="مشاهده جزئیات ' + p.name + '">' +
          '<div class="plate" style="background:color-mix(in srgb, var(' + SWATCH_VAR[p.swatch] + ') 14%, var(--surface-2));">' +
            tagHtml +
            '<img src="' + p.image + '" alt="' + p.name + '" loading="lazy">' +
          '</div>' +
        '</a>' +
        '<div class="body">' +
          '<span class="cat-line">' + p.cat + '</span>' +
          '<h3><a href="product.html?id=' + rid + '">' + p.name + '</a></h3>' +
          '<div class="row">' +
            '<span class="price">' + toFa(p.price.toLocaleString("en-US")) + '<small>تومان</small></span>' +
            '<button class="add" type="button" data-add>افزودن به سبد</button>' +
          '</div>' +
        '</div>' +
      '</article>'
    );
  }).join("");

  /* ---------- size + qty selectors ---------- */
  var selectedSize = null;
  var sizeGrid = document.getElementById("sizeGrid");
  sizeGrid.addEventListener("click", function(e){
    var chip = e.target.closest(".size-chip");
    if (!chip) return;
    sizeGrid.querySelectorAll(".size-chip").forEach(function(c){ c.classList.remove("selected"); });
    chip.classList.add("selected");
    selectedSize = chip.dataset.size;
  });

  var pdpQty = 1;
  var pdpQtyValue = document.getElementById("pdpQtyValue");
  document.getElementById("pdpQtyInc").addEventListener("click", function(){
    pdpQty = Math.min(10, pdpQty + 1);
    pdpQtyValue.textContent = toFa(pdpQty);
  });
  document.getElementById("pdpQtyDec").addEventListener("click", function(){
    pdpQty = Math.max(1, pdpQty - 1);
    pdpQtyValue.textContent = toFa(pdpQty);
  });

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

  document.getElementById("pdpAddToCart").addEventListener("click", function(){
    if (!selectedSize){
      sizeGrid.classList.remove("shake");
      void sizeGrid.offsetWidth;
      sizeGrid.classList.add("shake");
      showToast("لطفاً ابتدا سایز را انتخاب کنید");
      return;
    }
    var cartId = pid + "__" + selectedSize;
    var catWithSize = product.cat + " · سایز " + toFa(selectedSize);
    addToCart(cartId, product.name, catWithSize, product.price, product.swatch, pdpQty, product.image);
    bumpBadge();
    showToast(product.name + " به سبد اضافه شد");
    openDrawer();
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
