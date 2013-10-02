function closeBoxId(id){
    document.getElementById(id).style.display='none';
    document.getElementById('page-mask').style.display='none';

}

function openBoxId(id){
    document.getElementById(id).style.display='block';
    document.getElementById('page-mask').style.display='block';
}

function closeOpenBoxId(idClose, idOpen){
    document.getElementById('page-mask').style.display='none';	
    document.getElementById(idClose).style.display='none';
    document.getElementById(idOpen).style.display='block';
    document.getElementById('page-mask').style.display='block';
}

function FacebookLogin() {
    FB.login(function (response) {
        if (response.authResponse) {
            $.ajax({
                cache: false,
                async: true,
                type: "POST",
                url: '/Account/FbLogin',
                data: response.authResponse,
                dataType: "text",
                success: function (data) {
                    window.location.reload();
                }
            });
        }
    }, { scope: 'email' });
}

function FacebookLogout() {
    FB.logout(function (response) {

    });
}

(function($) {
    $(document).ready(function() {

$('.hastip').tooltipsy({
    alignTo: 'cursor',
    offset: [10, 10],
    css: {
        'padding': '10px',
        'max-width': '200px',
        'color': '#303030',
        'background-color': 'white',
        'border': '1px solid black',
        '-moz-box-shadow': '0 0 10px rgba(0, 0, 0, .5)',
        '-webkit-box-shadow': '0 0 10px rgba(0, 0, 0, .5)',
        'box-shadow': '10px 10px 5px #888',
        'text-shadow': 'none'
    }
});

$('.mask').click(function(event){
	$(this).hide();
	$('.white_content').hide();
});

$('#submit-deal').click(function(event){
  	$('.black_overlay').click(function() {
    		$('#light.white_content').hide();
		$('.black_overlay').hide();
  	});
	$('#light.white_content').click(function(event) {
  		event.stopPropagation();
	});
  	event.stopPropagation();
});

$('.social_connect_login_facebook').click(function(){
    FacebookLogin();
});



    });
})(jQuery);
