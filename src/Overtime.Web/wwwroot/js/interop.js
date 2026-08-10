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

// Swipe/drag support for the product-detail image gallery. Ported from the
// original assets/js/product.js drag-threshold logic; reports a direction
// (-1/+1) back to the Blazor component instead of mutating the DOM directly.
window.overtimeGallery = {
  attach: function (sliderEl, dotNetRef) {
    if (!sliderEl) return null;
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

// Scrolls the home page product carousel to a given item. Uses
// scrollIntoView's logical "inline" alignment instead of a transform, so it
// scrolls the correct physical direction under both LTR and RTL without any
// manual sign math.
window.overtimeCarousel = {
  scrollToIndex: function (trackEl, index) {
    if (!trackEl) return;
    var item = trackEl.children[index];
    if (item) item.scrollIntoView({ behavior: "smooth", inline: "start", block: "nearest" });
  }
};
