$(document).ready(function()
{    
	var pageselected = localStorage.getItem('selectedPage');
	console.log(pageselected);

	if(pageselected===null)
	{
		$('#content-section-block').load("overview.html");
	}
	else
	{
		$('#content-section-block').load(pageselected + '.html');
	}
	

	$('.page2-menu-options').on('click', function(e)
	{
        e.preventDefault();
		var page = $(this).attr('href');

		if(page == 'report_shiva' || page == 'report_trevor')
		{
			window.location.href = page + '.html';
		}
		else
		{
			$('#content-section-block').load(page + '.html');
		}
		
        return false;
	});
});
