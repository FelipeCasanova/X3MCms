function animation() {
  return {
    // make note that other events (like addClass/removeClass)
    // have different function input parameters
    enter: function(element, doneFn) {
      jQuery(element).fadeIn(1000, doneFn);
      
      // remember to call doneFn so that AngularJS
      // knows that the animation has concluded
    },

    move: function(element, doneFn) {
      jQuery(element).fadeIn(1000, doneFn);
    },

    leave: function(element, doneFn) {
      jQuery(element).fadeOut(1000, doneFn);
    }
  }
}

app.animation('.list-group-item', [animation]);
app.animation('.page-item', [animation]);
app.animation('.zone-item', [animation]);