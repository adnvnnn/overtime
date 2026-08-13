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

// Swipe/drag support for the hero slider and the product-detail image
// gallery. Ported from the original assets/js/product.js drag-threshold
// logic; reports a direction (-1/+1) back to the Blazor component instead
// of mutating the DOM directly.
window.overtimeGallery = {
  attach: function (sliderEl, dotNetRef) {
    if (!sliderEl || sliderEl.nodeType !== 1) return null;
    let dragging = false, moved = false, startX = 0, deltaX = 0;

    function pointerX(e) { return e.touches ? e.touches[0].clientX : e.clientX; }
    function dragStart(e) {
      dragging = true; moved = false; deltaX = 0; startX = pointerX(e);
      sliderEl.classList.add("dragging");
    }
    function dragMove(e) {
      if (!dragging) return;
      deltaX = pointerX(e) - startX;
      if (Math.abs(deltaX) > 4) moved = true;
    }
    function dragEnd() {
      if (!dragging) return;
      dragging = false;
      sliderEl.classList.remove("dragging");
      const threshold = sliderEl.clientWidth * 0.16;
      if (deltaX < -threshold) dotNetRef.invokeMethodAsync("OnSwipe", 1);
      else if (deltaX > threshold) dotNetRef.invokeMethodAsync("OnSwipe", -1);
    }
    // The hero slides are links now (clickable through to /products) —
    // dragging shouldn't also fire a click-through navigation on whatever
    // slide was under the cursor when the pointer came back up. Mirrors the
    // same fix the product carousel needed.
    function onClickCapture(e) {
      if (moved) {
        e.preventDefault();
        e.stopPropagation();
        moved = false;
      }
    }
    // Suppresses the browser's own native image-drag (ghost thumbnail),
    // which would otherwise race this custom drag — same fix as the carousel.
    function onDragStart(e) {
      e.preventDefault();
    }

    sliderEl.addEventListener("touchstart", dragStart, { passive: true });
    sliderEl.addEventListener("touchmove", dragMove, { passive: true });
    sliderEl.addEventListener("touchend", dragEnd);
    sliderEl.addEventListener("mousedown", function (e) { e.preventDefault(); dragStart(e); });
    window.addEventListener("mousemove", dragMove);
    window.addEventListener("mouseup", dragEnd);
    sliderEl.addEventListener("click", onClickCapture, true);
    sliderEl.addEventListener("dragstart", onDragStart);

    return {
      dispose: function () {
        sliderEl.removeEventListener("touchstart", dragStart);
        sliderEl.removeEventListener("touchmove", dragMove);
        sliderEl.removeEventListener("touchend", dragEnd);
        window.removeEventListener("mousemove", dragMove);
        window.removeEventListener("mouseup", dragEnd);
        sliderEl.removeEventListener("click", onClickCapture, true);
        sliderEl.removeEventListener("dragstart", onDragStart);
      }
    };
  }
};

// Drives the home page product carousel. Position is driven entirely by a
// JS `transform: translateX(...)` on the track — no native scrolling at
// all — with drag-to-index snapping based on *distance dragged* rather than
// release velocity. This sidesteps every native-scroll quirk the previous
// scrollLeft-based version kept running into: RTL's negative-scrollLeft
// sign model, CSS scroll-snap fighting a JS-driven scrollLeft, and
// background-tab timer throttling corrupting requestAnimationFrame-based
// momentum physics. Since the number of items visible at once changes at
// each responsive breakpoint, the reachable dot count and per-card step are
// measured from the real layout instead of assumed. Visibility is reported
// back so the Blazor side can pause auto-scroll while the carousel is
// off-screen.
window.overtimeCarousel = {
  attach: function (trackEl, dotNetRef) {
    if (!trackEl || trackEl.nodeType !== 1) return null;

    var viewport = trackEl.parentElement; // .carousel-viewport — the overflow:hidden clipping box
    var container = viewport ? viewport.parentElement : null; // .carousel-container — also hosts the arrows

    // translateX's sign convention flips under RTL: index 0 sits at the
    // *right* edge (content reads right-to-left), so moving to a later
    // index has to translate the track rightward (positive) — the opposite
    // of LTR's leftward (negative). Every index<->pixel conversion below
    // goes through this one sign instead of special-casing RTL at each site.
    var translateSign = getComputedStyle(trackEl).direction === "rtl" ? 1 : -1;

    var step = 0, maxIndex = 0, currentIndex = 0;

    function measure() {
      var items = trackEl.children;
      if (items.length < 2) {
        step = items.length ? items[0].getBoundingClientRect().width : 0;
        maxIndex = 0;
      } else {
        var r0 = items[0].getBoundingClientRect();
        var r1 = items[1].getBoundingClientRect();
        step = Math.abs(r1.left - r0.left);
        if (step <= 0) {
          maxIndex = items.length - 1;
        } else {
          // step is item-width + gap, but the *last* visible item doesn't
          // need a trailing gap — dividing the raw viewport width by step
          // alone always undercounts by one item (e.g. 4 items fit exactly,
          // but the division comes out just under 4 and floors to 3), which
          // is what was inflating the dot count by one on every breakpoint.
          // Adding one gap's worth back to the numerator corrects for that
          // missing trailing gap.
          var gap = Math.max(0, step - r0.width);
          var viewportWidth = viewport ? viewport.clientWidth : trackEl.clientWidth;
          var visibleCount = Math.max(1, Math.floor((viewportWidth + gap) / step));
          maxIndex = Math.max(0, items.length - visibleCount);
        }
      }
      if (currentIndex > maxIndex) currentIndex = maxIndex;
    }

    function translateFor(index) {
      return translateSign * index * step;
    }

    function applyTransform(animate) {
      trackEl.classList.toggle("snap", animate);
      trackEl.style.transform = "translateX(" + translateFor(currentIndex) + "px)";
    }

    function reportMaxIndex() {
      dotNetRef.invokeMethodAsync("OnMaxIndexChanged", maxIndex);
    }
    function reportCurrentIndex() {
      dotNetRef.invokeMethodAsync("OnCurrentIndexChanged", currentIndex);
    }

    measure();
    reportMaxIndex();
    applyTransform(false);

    var resizeObserver = new ResizeObserver(function () {
      var before = currentIndex;
      measure();
      applyTransform(false);
      reportMaxIndex();
      if (currentIndex !== before) reportCurrentIndex();
    });
    resizeObserver.observe(trackEl);

    var intersectionObserver = new IntersectionObserver(function (entries) {
      dotNetRef.invokeMethodAsync("OnVisibilityChanged", entries[entries.length - 1].isIntersecting);
    }, { threshold: 0.3 });
    intersectionObserver.observe(trackEl);

    // Pause auto-scroll while the pointer is over the carousel (buttons and
    // dots included, since they're siblings of the viewport under this same
    // container) so it doesn't yank a slide away mid-read or mid-hover.
    var onEnter = function () { dotNetRef.invokeMethodAsync("OnHoverChanged", true); };
    var onLeave = function () { dotNetRef.invokeMethodAsync("OnHoverChanged", false); };
    if (container) {
      container.addEventListener("mouseenter", onEnter);
      container.addEventListener("mouseleave", onLeave);
    }

    // Moves to the given index (clamped), animating unless told not to —
    // used both by the drag-release snap below and by button/dot/auto-scroll
    // navigation from the Blazor side (see the returned goTo()).
    function goToIndex(index, animate) {
      index = Math.max(0, Math.min(maxIndex, index));
      var changed = index !== currentIndex;
      currentIndex = index;
      applyTransform(animate !== false);
      if (changed) reportCurrentIndex();
    }

    // Uses the Pointer Events API with setPointerCapture (plus a
    // window-level fallback below in case capture doesn't retarget the
    // release back to the track) rather than mousedown/window-mousemove/
    // window-mouseup — the older pattern gets permanently stuck mid-drag if
    // the pointer leaves the browser window before it's released.
    //
    // Unlike a native scroll container, nothing here scrolls natively for
    // touch either — touch-action:pan-y (CSS) hands horizontal gestures to
    // this same code instead of the browser, so touch goes through the exact
    // same path as mouse/pen instead of a separate native fallback.
    var activePointerId = null, dragged = false, startX = 0, startTranslate = 0, liveTranslate = 0;
    var pressedLink = null;
    var DRAG_THRESHOLD = 0.18; // fraction of one card's width that counts as "committed to moving"

    function onPointerDown(e) {
      activePointerId = e.pointerId;
      dragged = false;
      startX = e.clientX;
      startTranslate = translateFor(currentIndex);
      liveTranslate = startTranslate;
      // Captured now, before setPointerCapture below can affect anything:
      // once the pointer is captured, the native `click` that normally
      // follows a release gets retargeted to trackEl itself instead of
      // whatever link was actually under the cursor (per the Pointer Events
      // spec — capture redirects the whole event sequence to the capturing
      // element). That means a genuine, no-drag click would never actually
      // reach the card's <a> to navigate it, so onPointerEnd below drives
      // navigation from this reference directly instead of trusting the
      // click to land on the right element.
      pressedLink = e.target.closest("a[href]");
      trackEl.classList.add("dragging");
      try {
        trackEl.setPointerCapture(e.pointerId);
      } catch (err) { /* capture unsupported/rejected — window-level fallback below still resolves the drag */ }
      // Belt-and-suspenders alongside .carousel-track's CSS user-select:none —
      // stops the browser starting a text/image selection right from the
      // gesture that kicks off the drag.
      e.preventDefault();
    }
    function onPointerMove(e) {
      if (e.pointerId !== activePointerId) return;
      var delta = e.clientX - startX;
      if (Math.abs(delta) > 4) dragged = true;

      // Content follows the cursor 1:1 — this is a direct transform, not a
      // native scroll fighting for control, so unlike the old scrollLeft
      // approach there's no need for an amplified drag multiplier to feel
      // responsive.
      var next = startTranslate + delta;
      var lo = Math.min(0, translateFor(maxIndex));
      var hi = Math.max(0, translateFor(maxIndex));
      liveTranslate = Math.max(lo, Math.min(hi, next));
      trackEl.classList.remove("snap");
      trackEl.style.transform = "translateX(" + liveTranslate + "px)";
      e.preventDefault();
    }
    function onPointerEnd(e) {
      if (e.pointerId !== activePointerId) return;
      activePointerId = null;
      try {
        if (trackEl.hasPointerCapture(e.pointerId)) trackEl.releasePointerCapture(e.pointerId);
      } catch (err) { /* already released/invalid — nothing to clean up */ }
      trackEl.classList.remove("dragging");

      // Only follow the link on a genuine no-drag release — this is the
      // real click-vs-drag decision now, not the onClickCapture guard below
      // (whose click target isn't reliable under pointer capture).
      var linkToActivate = !dragged ? pressedLink : null;
      pressedLink = null;

      if (!dragged || step <= 0) {
        applyTransform(true);
        if (linkToActivate) linkToActivate.click();
        return;
      }
      // How far past the threshold decides how many cards to move, not just
      // whether to move one — a fast, long drag can skip past several cards
      // in one release, matching how far it was actually dragged rather than
      // simulating a coast afterward.
      var indexDelta = (liveTranslate - startTranslate) / (translateSign * step);
      var indexChange = Math.abs(indexDelta) > DRAG_THRESHOLD
        ? Math.sign(indexDelta) * Math.ceil(Math.abs(indexDelta))
        : 0;
      goToIndex(currentIndex + indexChange, true);
    }
    // Belt-and-suspenders: onPointerEnd above already decides whether to
    // navigate (dragged vs not) and drives it directly rather than relying
    // on this click, since pointer capture retargets it to trackEl rather
    // than the actual card that was under the cursor. This just mops up
    // that retargeted click after a real drag so it can't do anything else.
    function onClickCapture(e) {
      if (dragged) {
        e.preventDefault();
        e.stopPropagation();
        dragged = false;
      }
    }
    // The cards contain <a> links and <img>s, which the browser will happily
    // pick up as its own native drag operation (ghost thumbnail following
    // the cursor) the moment the pointer moves past their built-in drag
    // threshold — racing our drag above and making it look broken.
    // Suppressing dragstart kills the native drag entirely; plain clicks
    // never fire dragstart, so links stay clickable.
    function onDragStart(e) {
      e.preventDefault();
    }

    trackEl.addEventListener("pointerdown", onPointerDown);
    trackEl.addEventListener("pointermove", onPointerMove);
    trackEl.addEventListener("pointerup", onPointerEnd);
    trackEl.addEventListener("pointercancel", onPointerEnd);
    trackEl.addEventListener("click", onClickCapture, true);
    trackEl.addEventListener("dragstart", onDragStart);
    // Belt-and-suspenders for setPointerCapture: onPointerEnd checks the
    // pointerId and is a no-op once the drag already resolved via trackEl's
    // own listener, so this only ever does anything in the rare case
    // capture didn't retarget the release back to the track — the exact
    // scenario that leaves a drag permanently "stuck" (dragging class/cursor
    // pinned) if nothing else catches it.
    window.addEventListener("pointerup", onPointerEnd);
    window.addEventListener("pointercancel", onPointerEnd);

    return {
      // Called from the Blazor side (buttons, dots, auto-scroll) via the
      // IJSObjectReference this attach() call returns.
      goTo: function (index, smooth) {
        if (activePointerId !== null) return; // a live drag owns the track — don't fight it
        goToIndex(index, smooth !== false);
      },
      dispose: function () {
        resizeObserver.disconnect();
        intersectionObserver.disconnect();
        trackEl.removeEventListener("pointerdown", onPointerDown);
        trackEl.removeEventListener("pointermove", onPointerMove);
        trackEl.removeEventListener("pointerup", onPointerEnd);
        trackEl.removeEventListener("pointercancel", onPointerEnd);
        trackEl.removeEventListener("click", onClickCapture, true);
        trackEl.removeEventListener("dragstart", onDragStart);
        window.removeEventListener("pointerup", onPointerEnd);
        window.removeEventListener("pointercancel", onPointerEnd);
        if (container) {
          container.removeEventListener("mouseenter", onEnter);
          container.removeEventListener("mouseleave", onLeave);
        }
      }
    };
  }
};
