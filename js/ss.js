$(document).ready(function()
{    
	// this block happens when the page first loads up
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
	
	// this block happens only when a .page2-menu-options element is clicked
	$('.page2-menu-options').on('click', function(e)
	{
        e.preventDefault();
		var page = $(this).attr('href');
		
		// this will update localStorage with the last page clicked
		// if a person navigates away and then clicks back it will
		// open the last page they were on
		localStorage.setitem("selectedPage", page);

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
