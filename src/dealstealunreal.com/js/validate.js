//clear fields
function clearFields(){
	jQuery('#deal_title').val('');
	jQuery('#deal_retailer').val('');
	jQuery('#deal_url').val('');
	jQuery('#deal_price').val('');
	jQuery('#deal_image_url').val('');
	jQuery('#deal_description').val('');
	
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
	//alert("Got a deal? Please login to submit.");
	window.location.href=url;
	//window.location.href=url+"/wp-admin/";
}


function openBox(){

	jQuery('#light').css('display' , 'block');
	jQuery('#fade').css('display' , 'block');
	
	jQuery('.error1').each(function(){
		jQuery(this).css('display' , 'none');						   
	})
	clearFields();
}

function addDeal(){
	var deal_title = jQuery('#deal_title').val();
	var deal_retailer = jQuery('#deal_retailer').val();
	var deal_url = jQuery('#deal_url').val();
	var deal_price = jQuery('#deal_price').val();
	var deal_image_url = jQuery('#deal_image_url').val();
	var deal_description = jQuery('#deal_description').val();
	
	var pluginPath = jQuery('#pluginPath').val();
	var pluginPath = jQuery('#pluginPath').val();

    var maxWords = 200;

    if(deal_description.split(/\b[\s,\.-:;]*/).length > maxWords ){
        jQuery('#dealDescriptionError').css('display' , 'block');
        jQuery('#deal_description').focus();
        return false;
    }else{
        jQuery('#dealDescriptionError').css('display' , 'none');
    }

	if(deal_title==""){
		jQuery('#dealTitleError').css('display' , 'block');
		jQuery('#deal_title').focus();
		return false;
	}else{
		jQuery('#dealTitleError').css('display' , 'none');
	}
	if(deal_retailer==""){
		jQuery('#dealRetailerError').css('display' , 'block');
		jQuery('#deal_retailer').focus();
		return false;
	}else{
		jQuery('#dealRetailerError').css('display' , 'none');
	}
	
	if(deal_url==""){
		jQuery('#dealUrlError').css('display' , 'block');
		jQuery('#deal_url').focus();
		return false;
	}else{
		jQuery('#dealUrlError').css('display' , 'none');
	}
	
	if(deal_price==""){
		jQuery('#dealPriceError').css('display' , 'block');
		jQuery('#deal_price').focus();
		return false;
	}else{
		jQuery('#dealPriceError').css('display' , 'none');
	}

    if (deal_image_url == "") {
        jQuery('#dealImageError').css('display', 'block');
        jQuery('#deal_image_url').focus();
        return false;
    } else {
        jQuery('#dealImageError').css('display', 'none');
    }


    var form = $('#submitdealform');
$.ajax({
    cache: false,
    async: true,
    type: "POST",
    url: form.attr('action'),
    data: form.serialize(),
    dataType: "text",
    success: function (data) {
        clearFields();
        window.location.reload();
    }
});
		
		
}