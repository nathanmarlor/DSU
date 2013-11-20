//clear fields
function clearFields(){
	jQuery('#deal_title').val('');
	jQuery('#deal_retailer').val('');
	jQuery('#deal_url').val('');
	jQuery('#deal_price').val('');
	jQuery('#deal_image_url').val('');
	jQuery('#deal_description').val('');
	jQuery('#topError').text('');
}

//close popup
function closeBox(){
	document.getElementById('light').style.display='none';document.getElementById('fade').style.display='none';	
}

//show hide login divs
function showfb(){
	$('#fbForm').css('display' , 'block');
	$('#fbLoginForm').css('display' , 'none');
}

function showform(){
	$('#fbLoginForm').css('display' , 'block');
	$('#fbForm').css('display' , 'none');	
}


function loginFirst(url,info){
	alert(info);
	window.location.href=url;
}

function openBox(){

	jQuery('#light').css('display' , 'block');
	jQuery('#fade').css('display' , 'block');

    jQuery('.error1').each(function() {
        jQuery(this).css('display', 'none');
    });
    
	clearFields();
}

function addDeal(){

    var form = $('#submitdealform');
    $.ajax({
        cache: false,
        async: true,
        type: "POST",
        url: form.attr('action'),
        data: form.serialize(),
        dataType: "text",
        success: function(data) {
            clearFields();
            window.location.reload();
        },
        error: function(xhr, status) {
            jQuery('#topError').text("Error: " + xhr.responseText);
            jQuery('#topError').css('display', 'block');
        }
    });


}