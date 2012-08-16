/* Default class modification */
$.extend( $.fn.dataTableExt.oStdClasses, {
	"sWrapper": "dataTables_wrapper form-inline"
} );

/* API method to get paging information */
$.fn.dataTableExt.oApi.fnPagingInfo = function ( oSettings )
{
	return {
		"iStart":         oSettings._iDisplayStart, // 起始记录行数
		"iEnd":           oSettings.fnDisplayEnd(), // 结束记录行数
		"iLength":        oSettings._iDisplayLength, // 该页显示记录数
		"iTotal":         oSettings.fnRecordsTotal(), // 总共记录数
		"iFilteredTotal": oSettings.fnRecordsDisplay(),
		"iPage":          Math.ceil( oSettings._iDisplayStart / oSettings._iDisplayLength ), // 当前页索引 0开始
		"iTotalPages":    Math.ceil( oSettings.fnRecordsDisplay() / oSettings._iDisplayLength ) // 总共索引页数
	};
}

/* Bootstrap style pagination control */
$.extend($.fn.dataTableExt.oPagination, {
    "iFullNumbersShowPages": 5,
    "bootstrap": {
        "fnInit": function (oSettings, nPaging, fnDraw) {
            var oLang = oSettings.oLanguage.oPaginate;
            var fnClickHandler = function (e) {
                e.preventDefault();
                if (oSettings.oApi._fnPageChange(oSettings, e.data.action)) {
                    fnDraw(oSettings);
                }
            };

            $(nPaging).addClass('pagination').append(
				'<ul>' +
					'<li class="first disabled"><a href="javascript:void(0)">&larr; ' + oLang.sFirst + '</a></li>' +
                    '<li class="prev disabled" id="bsp_prev"> <a href="javascript:void(0)">&larr; ' + oLang.sPrevious + '</a></li>' +
                    '<li class="next disabled" id="bsp_next"> <a href="javascript:void(0)">' + oLang.sNext + ' &rarr; </a></li>' +
					'<li class="last disabled"> <a href="javascript:void(0)">' + oLang.sLast + ' &rarr; </a></li>' +
				'</ul>'
			);
            var els = $('a', nPaging);
            $(els[0]).bind('click.DT', { action: "first" }, fnClickHandler);

            $(els[1]).bind( 'click.DT', { action: "previous" }, fnClickHandler );
            $(els[2]).bind('click.DT', { action: "next" }, fnClickHandler);

            $(els[3]).bind('click.DT', { action: "last" }, fnClickHandler);

        },

        "fnUpdate": function (oSettings, fnDraw) {
            var iListLength = 10;// $.fn.dataTableExt.oPagination.iFullNumbersShowPages;

            var oPaging = oSettings.oInstance.fnPagingInfo();
            var an = oSettings.aanFeatures.p;
            var i, j, sClass, iStart, iEnd, iHalf = Math.floor(iListLength / 2);

            if (oPaging.iTotalPages < iListLength) {
                iStart = 1;
                iEnd = oPaging.iTotalPages;
            }
            else if (oPaging.iPage <= iHalf) {
                iStart = 1;
                iEnd = iListLength;
            } else if (oPaging.iPage >= (oPaging.iTotalPages - iHalf)) {
                iStart = oPaging.iTotalPages - iListLength + 1;
                iEnd = oPaging.iTotalPages;
            } else {
                iStart = oPaging.iPage - iHalf + 1;
                iEnd = iStart + iListLength - 1;
            }

            for (i = 0, iLen = an.length; i < iLen; i++) {
                // Remove the middle elements
                $('li:gt(1)', an[i]).filter(':not(:last)').filter(':not(#bsp_next)').remove();

                // Add the new list items and their event handlers
                for (j = iStart; j <= iEnd; j++) {
                    sClass = (j == oPaging.iPage + 1) ? 'class="active"' : '';
                    $('<li ' + sClass + '><a href="javascript:void(0)">' + j + '</a></li>')
						.insertBefore($('li#bsp_next', an[i])[0])
						.bind('click', function (e) {
						    e.preventDefault();
						    oSettings._iDisplayStart = (parseInt($('a', this).text(), 10) - 1) * oPaging.iLength;
						    fnDraw(oSettings);
						});
                }

                // Add / remove disabled classes from the static elements
                if (oPaging.iPage === 0) {
                    $('li:first', an[i]).addClass('disabled');
                    $('li#bsp_prev', an[i]).addClass('disabled');
                    $('li#bsp_prev', an[i]).addClass('active');
                } else {
                    $('li:first', an[i]).removeClass('disabled');
                    $('li#bsp_prev', an[i]).removeClass('disabled');
                    $('li#bsp_prev', an[i]).removeClass('active');
                }

                if (oPaging.iPage === oPaging.iTotalPages - 1 || oPaging.iTotalPages === 0) {
                    $('li:last', an[i]).addClass('disabled');
                    $('li#bsp_next', an[i]).addClass('disabled');
                    $('li#bsp_next', an[i]).addClass('active');
                } else {
                    $('li:last', an[i]).removeClass('disabled');
                    $('li#bsp_next', an[i]).removeClass('disabled');
                    $('li#bsp_next', an[i]).removeClass('active');
                }
            }
        }
    }
});

///* Table initialisation */
//$(document).ready(function() {
//	$('#example').dataTable( {
//		"sDom": "<'row'<'span6'l><'span6'f>r>t<'row'<'span6'i><'span6'p>>",
//		"sPaginationType": "bootstrap",
//		"oLanguage": {
//			"sLengthMenu": "_MENU_ records per page"
//		}
//	} );
//} );