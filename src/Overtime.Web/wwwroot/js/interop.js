window.overtimeTheme = {
  current: function () {
    return document.documentElement.getAttribute("data-theme") === "dark" ? "dark" : "light";
  },
  toggle: function () {
    var next = this.current() === "dark" ? "light" : "dark";
    document.documentElement.setAttribute("data-theme", next);
    try { localStorage.setItem("overtime_theme", next); } catch (e) {}
    return next;
  }
};

// Locks background scroll while the mobile off-canvas nav drawer is open, so the
// page behind the fixed-position drawer/backdrop can't be dragged underneath it.
window.overtimeNav = {
  setBodyScrollLock: function (locked) {
    document.body.classList.toggle("nav-locked", !!locked);
  }
};

// Swipe/drag support for the product-detail image gallery. Ported from the
// original assets/js/product.js drag-threshold logic; reports a direction
// (-1/+1) back to the Blazor component instead of mutating the DOM directly.
window.overtimeGallery = {
  attach: function (sliderEl, dotNetRef) {
    if (!sliderEl || sliderEl.nodeType !== 1) return null;
    let dragging = false, startX = 0, deltaX = 0;

    function pointerX(e) { return e.touches ? e.touches[0].clientX : e.clientX; }
    function dragStart(e) {
      dragging = true; deltaX = 0; startX = pointerX(e);
      sliderEl.classList.add("dragging");
    }
    function dragMove(e) {
      if (!dragging) return;
      deltaX = pointerX(e) - startX;
    }
    function dragEnd() {
      if (!dragging) return;
      dragging = false;
      sliderEl.classList.remove("dragging");
      const threshold = sliderEl.clientWidth * 0.16;
      if (deltaX < -threshold) dotNetRef.invokeMethodAsync("OnSwipe", 1);
      else if (deltaX > threshold) dotNetRef.invokeMethodAsync("OnSwipe", -1);
    }

    sliderEl.addEventListener("touchstart", dragStart, { passive: true });
    sliderEl.addEventListener("touchmove", dragMove, { passive: true });
    sliderEl.addEventListener("touchend", dragEnd);
    sliderEl.addEventListener("mousedown", function (e) { e.preventDefault(); dragStart(e); });
    window.addEventListener("mousemove", dragMove);
    window.addEventListener("mouseup", dragEnd);

    return {
      dispose: function () {
        sliderEl.removeEventListener("touchstart", dragStart);
        sliderEl.removeEventListener("touchmove", dragMove);
        sliderEl.removeEventListener("touchend", dragEnd);
        window.removeEventListener("mousemove", dragMove);
        window.removeEventListener("mouseup", dragEnd);
      }
    };
  }
};

// Drives the home page product carousel. Scrolling uses scrollIntoView's
// logical "inline" alignment instead of a transform, so it scrolls the
// correct physical direction under both LTR and RTL without any manual sign
// math. Since the number of items visible at once changes at each
// responsive breakpoint, the reachable dot count is measured from the real
// layout instead of assumed, so the dots always match how many distinct
// positions the carousel can actually scroll to. Visibility is also
// reported back so the Blazor side can pause auto-scroll while the
// carousel is off-screen — scrollIntoView would otherwise yank the whole
// page back to it every few seconds.
window.overtimeCarousel = {
  attach: function (trackEl, dotNetRef) {
    if (!trackEl || trackEl.nodeType !== 1) return null;

    function computeMaxIndex() {
      var items = trackEl.children;
      if (items.length < 2) return 0;
      var r0 = items[0].getBoundingClientRect();
      var r1 = items[1].getBoundingClientRect();
      var step = Math.abs(r1.left - r0.left);
      if (step <= 0) return items.length - 1;
      var visibleCount = Math.max(1, Math.floor(trackEl.clientWidth / step));
      return Math.max(0, items.length - visibleCount);
    }

    function reportMaxIndex() {
      dotNetRef.invokeMethodAsync("OnMaxIndexChanged", computeMaxIndex());
    }

    reportMaxIndex();
    var resizeObserver = new ResizeObserver(reportMaxIndex);
    resizeObserver.observe(trackEl);

    var intersectionObserver = new IntersectionObserver(function (entries) {
      dotNetRef.invokeMethodAsync("OnVisibilityChanged", entries[entries.length - 1].isIntersecting);
    }, { threshold: 0.3 });
    intersectionObserver.observe(trackEl);

    return {
      dispose: function () {
        resizeObserver.disconnect();
        intersectionObserver.disconnect();
      }
    };
  },
  scrollToIndex: function (trackEl, index) {
    if (!trackEl) return;
    var item = trackEl.children[index];
    if (item) item.scrollIntoView({ behavior: "smooth", inline: "start", block: "nearest" });
  }
};
