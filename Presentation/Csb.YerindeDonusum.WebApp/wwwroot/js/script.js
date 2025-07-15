

// <!-- header/// -->
$(window).scroll(function() {
  if ($(this).scrollTop() > 1) {
      $('.header-section').addClass("sticky").fadeIn(1000);
      $(".main_logo").attr("src","assets/img/Logo/logo-b.png");
  } else {
      $('.header-section').removeClass("sticky");
      $(".main_logo").attr("src","assets/img/Logo/logo-w.png");
      $(".innerpage .main_logo").attr("src","assets/img/Logo/logo-b.png");

  }
});
$(".innerpage .main_logo").attr("src","assets/img/Logo/logo-b.png");
// <!-- header/// -->



// toggle menu -------------------
$(".header-section .links-sec .toggle-menu button").click(function(){
  $(".mobile-header .menu-content").toggleClass('is-active');  
});
$(".close_btn").click(function(){
  $(".mobile-header .menu-content").toggleClass('is-active');  
}); 




var forEach=function(t,o,r){if("[object Object]"===Object.prototype.toString.call(t))for(var c in t)Object.prototype.hasOwnProperty.call(t,c)&&o.call(r,t[c],c,t);else for(var e=0,l=t.length;l>e;e++)o.call(r,t[e],e,t)};

    var hamburgers = document.querySelectorAll(".hamburger");
    if (hamburgers.length > 0) {
      forEach(hamburgers, function(hamburger) {
        hamburger.addEventListener("click", function() {
          this.classList.toggle("is-active");
          $(".header-section .links").toggleClass('is-active'); 
          $(".header-section .overlay").toggleClass('ac'); 
          
        }, false);
      });
    }
// toggle menu -------------------
 
// open modal -------------------
$(window).on('load', function() {
  // $('#login').modal('show');
  if($(".manual-button").length > 0)
    changeDocumentsLinksOnMobile();
}); 
// open modal -------------------
function getMobileOperatingSystem() {
  var userAgent = navigator.userAgent || navigator.vendor || window.opera;

  // Windows Phone must come first because its UA also contains "Android"
  if (/windows phone/i.test(userAgent)) {
      return "Windows Phone";
  }

  if (/android/i.test(userAgent)) {
      return "Android";
  }

  // iOS detection from: http://stackoverflow.com/a/9039885/177710
  if (/iPad|iPhone|iPod/.test(userAgent) && !window.MSStream) {
      return "iOS";
  }

  return "unknown";
}

function changeDocumentsLinksOnMobile() {
  if (getMobileOperatingSystem() === "Android" || getMobileOperatingSystem() === "iOS") {
          var a = $(".manual-button");
          a.removeAttr('data-fancybox');
          a.attr('target', '_blank');
  }
}
// nice select --------------------
$(document).ready(function() {
  $('.nice-select').niceSelect();
});  
// nice select --------------------

 
// page active --------------------
$(document).ready(function(){
  $(function($) {
     var path = window.location.href;
     $('.menu .navbar .links ul li.list_item a').each(function() {
     if (this.href === path) {
        $(this).addClass('active');
     }
     });
  });
});
// page active --------------------
 
// select2  --------------------

$(document).ready(function() {
	$('.js-example-basic-multiple').select2();
	$('.js-example-basic-single').select2(); 
	  $('.withoutSearch').select2();
  }); 
  $(document).ready(function() {
	$('select').niceSelect();
  });
  // select2  --------------------


  // file upload  --------------------
  var $fileInput = $('.file-input');
var $droparea = $('.file-drop-area');

// highlight drag area
$fileInput.on('dragenter focus click', function() {
  $droparea.addClass('is-active');
});

// back to normal state
$fileInput.on('dragleave blur drop', function() {
  $droparea.removeClass('is-active');
});

// change inner text
$fileInput.on('change', function() {
  var filesCount = $(this)[0].files.length;
  var $textContainer = $(this).prev();

  if (filesCount === 1) {
    // if single file is selected, show file name
    var fileName = $(this).val().split('\\').pop();
    $textContainer.text(fileName);
  } else {
    // otherwise show number of files
    $textContainer.text(filesCount + ' files selected');
  }
});
  // file upload  --------------------

  // show password -------------------

$(document).ready(function() {
	$("#show_hide_password a").on('click', function(event) {
		event.preventDefault();
		if($('#show_hide_password input').attr("type") == "text"){
			$('#show_hide_password input').attr('type', 'password');
			$('#show_hide_password i').addClass( "fa-eye-slash" );
			$('#show_hide_password i').removeClass( "fa-eye" );
		}else if($('#show_hide_password input').attr("type") == "password"){
			$('#show_hide_password input').attr('type', 'text');
			$('#show_hide_password i').removeClass( "fa-eye-slash" );
			$('#show_hide_password i').addClass( "fa-eye" );
		}
	});
  });
  // show password -------------------
  
  // menu -------------------

  // menu -------------------
