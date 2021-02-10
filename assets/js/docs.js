
$(window).on('load resize', function() {
   
    //Add/remove class based on browser size when load/resize
    var w = $(window).width();

	if(w >= 1200) {
	    // if larger 
	    $('#docs-sidebar').addClass('sidebar-visible').removeClass('sidebar-hidden');
	} else {
	    // if smaller
	    $('#docs-sidebar').addClass('sidebar-hidden').removeClass('sidebar-visible');
	}
});


$(document).ready(function() {
	/* ====== Toggle Sidebar ======= */
	
	$('#docs-sidebar-toggler').on('click', function(){
	
		if ( $('#docs-sidebar').hasClass('sidebar-visible') ) {

			  $("#docs-sidebar").removeClass('sidebar-visible').addClass('sidebar-hidden');
			
			
		} else {

			  $("#docs-sidebar").removeClass('sidebar-hidden').addClass('sidebar-visible');
			
		}
		
    });

	$('#search-form-main').submit(function(e) { 
		var $inputs = $('#search-form-main :input');
   
		$inputs.each(function() {
		   e.preventDefault(); // Cancel the submit
		   return false; // Exit the .each loop
		});
   });

	//setup before functions
	var typingTimer;                //timer identifier
	var doneTypingInterval = 1500;  //time in ms, 1.5 second for example
	var $input = $('#searchInput');

	//on keyup, start the countdown
	$input.on('keyup', function () {
	clearTimeout(typingTimer);
	typingTimer = setTimeout(doneTyping, doneTypingInterval);
	});

	//on keydown, clear the countdown 
	$input.on('keydown', function () {
	clearTimeout(typingTimer);
	});

	//user is "finished typing," do something
	function doneTyping () {
		var searchTerm = $('#searchInput').val();
		if(searchTerm ==="") return;
		
		window.find(searchTerm,0,0,0,0,0,1);
	}

	$('#searchButton').on('click', function(){
	
		var searchTerm = $('#searchInput').val();
		if(searchTerm ==="") return;
		
		window.find(searchTerm,0,0,0,0,0,1);
		
    });

    /* ====== Activate scrollspy menu ===== */
    $('body').scrollspy({target: '#docs-nav', offset: 100});
    
    
    
    /* ===== Smooth scrolling ====== */
	$('#docs-sidebar a.scrollto').on('click', function(e){
        //store hash
        var target = this.hash;    
        e.preventDefault();
		$('body').scrollTo(target, 800, {offset: -69, 'axis':'y'});
		
		//Collapse sidebar after clicking
		if ($('#docs-sidebar').hasClass('sidebar-visible') && $(window).width() < 1200){
			$('#docs-sidebar').removeClass('sidebar-visible').addClass('slidebar-hidden');
		}
		
	});
	
	/* wmooth scrolling on page load if URL has a hash */
	if(window.location.hash) {
		var urlhash = window.location.hash;
		$('body').scrollTo(urlhash, 800, {offset: -69, 'axis':'y'});
	}
	
	
	/* Bootstrap lightbox */
    /* Ref: http://ashleydw.github.io/lightbox/ */

    $(document).delegate('*[data-toggle="lightbox"]', 'click', function(e) {
        e.preventDefault();
        $(this).ekkoLightbox();
    }); 

    
    

});