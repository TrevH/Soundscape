$(document).ready(function()
{    
	$('.page2-menu-options').on('click', function(e)
	{
        e.preventDefault();
		var page = $(this).attr('href');
		$('#content-section-block').load(page + '.html');
        return false;
	});
});
