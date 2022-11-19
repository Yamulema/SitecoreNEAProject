function createSearchElement(title, content, url, content2search, onClickEventTitle, onClickEventUrl) {

    var termsplit = content2search.split(" ");
    var strTitle = "";
    var strContent = "";
    if (termsplit.length > 1) {
        strContent = content;
        strTitle = title;
        for (var indexsplit in termsplit) {
            var item = termsplit[indexsplit];
            strTitle = strTitle.replace(new RegExp('\\b' + item + '\\b', 'gi'), '<b>$&</b>');
            if (content != null) {
                strContent = strContent.replace(new RegExp('\\b' + item + '\\b', 'gi'), '<b>$&</b>');
            } else {
                strContent = "";
            }
        }
    }
    else {
        strTitle = title.replace(new RegExp('\\b' + content2search + '\\b', 'gi'), '<b>$&</b>');
        if (content != null) {
            strContent = content.replace(new RegExp('\\b' + content2search + '\\b', 'gi'), '<b>$&</b>');
        } else {
            strContent = "";
        }
    }
    return $('<div />', {
        class: 'show-for-medium medium-1 large-1 columns',
        text: ""
    }
    ).append("&nbsp;").add(
        $('<div />', {
            class: 'small-12 medium-10 large-10 columns float-center'
        }
        ).append(
            $('<div />', {
                class: 'search_item'
            }
            ).append(
                $('<a />', {
                    html: '<h4>' + strTitle + '</h4>',
                    href: url,
                    onclick: onClickEventTitle
                }
                ).add(
                    $('<p />', {
                        class: 'page_content',
                        html: strContent
                    }
                    )
                ).add(
                    $('<a />', {
                        class: 'page_title',
                        href: url,
                        text: url,
                        onclick: onClickEventUrl
                    }
                    )
                )
            )
        )
    ).add(
        $('<div />', {
            class: 'show-for-medium medium-1 large-1 columns'
        }
        )
    )
}

function createPagination(paginator, toSearch) {
    var previous_message = $("#searchMessages #previous");
    var next_message = $("#searchMessages #next");
    var you_are_message = $("#searchMessages #youAre");

    var unordererList = $('<ul />', {
        class: 'pagination',
        role: 'navigation',
        ariaLabel: "Pagination"
    }
    );

    //var totalPages = Math.ceil(elements.length/pageSize);
    //console.log('creating li in ul');
    //console.log(totalPages); 
    if (1 == paginator.page) {
        unordererList.append(
            $('<li />', {
                class: 'pagination-previous disabled',
                html: '&lt; ' + previous_message.text().split(" ")[0] + ' <span class="show-for-sr">' + previous_message.text().split(" ")[1] + '</span>'
            }
            )
        );
    } else {
        unordererList.append(
            $('<li />', {
                class: 'pagination-previous',
                html: '<a aria-label="' + previous_message.text() + '">&lt; ' + previous_message.text().split(" ")[0] + ' <span class="show-for-sr">' + previous_message.text().split(" ")[1] + '</span></a>'
            }
            ).click(function (e) {
                doSearch(toSearch, parseInt(paginator.page) - 1)
                //do stuff here...
            })
        );
    }

    for (i = 0; i < paginator.total_pages; i++) {
        if (i + 1 == paginator.page) {
            unordererList.append(
                $('<li />', {
                    class: 'current',
                    id: i + 1,
                    html: '<span class="show-for-sr">' + you_are_message.text() + '</span>' + (i + 1) + '</a>'
                }
                ).click(function (e) {
                    doSearch(toSearch, e.currentTarget.id)
                    //do stuff here...
                })
            );
        } else {
            unordererList.append(
                $('<li />', {
                    id: i + 1,
                    html: '<a aria-label="Page ' + (i + 1) + '">' + (i + 1) + '</a>'
                }
                ).click(function (e) {
                    doSearch(toSearch, e.currentTarget.id)
                    //do stuff here...
                })
            );
        }

    }

    if (paginator.page == paginator.total_pages) {
        unordererList.append(
            $('<li />', {
                class: 'pagination-next disabled',
                html: next_message.text() + ' &gt;'
            }
            )
        );
    } else {
        unordererList.append(
            $('<li />', {
                class: 'pagination-next',
                html: '<a aria-label="Next page">' + next_message.text() + ' &gt;</a>'
            }
            ).click(function (e) {
                doSearch(toSearch, parseInt(paginator.page) + 1)
            })
        );
    }



    return $('<div />', {
        class: 'show-for-medium medium-1 large-1 columns',
        text: ""
    }
    ).append("&nbsp;").add(
        $('<div />', {
            class: 'small-12 medium-10 large-10 columns float-center'
        }
        ).append(
            $('<div />', {
                class: 'docs-code-live'
            }
            ).append(
                unordererList
            )
        )
    ).add(
        $('<div />', {
            class: 'show-for-medium medium-1 large-1 columns'
        }
        )
    )
}

var search_elements = [];

function doSearch(toSearch, page) {

    var search_url = '/api/SearchSeiumb/GlobalSearch';
    //  var search_url = 'http://localhost:3000/results';
    //  var searchData = '';

    if (typeof page === 'undefined' || page === null) {
        var page = 1;
        $.ajax({
            type: 'POST',
            url: search_url,
            data: { searchText: toSearch },
            dataType: 'json',
            error: function (data) {
            },
            success: function (data) {
                search_elements = data.results;
                showResults(toSearch, search_elements);
            }
        });

        /*   
        $.ajax({
            type: 'GET',
            url: search_url,
            //data: searchData,
            dataType: 'json',
            error: function (data) {
            },
            success: function (data) {
                search_elements = data;
                showResults(toSearch, search_elements, page);
            }
        });
        */

    } else {
        showResults(toSearch, search_elements, page);
    }
}

function showResults(toSearch, data, page) {

    if (typeof page === 'undefined' || page === null) {
        var page = 1;
    }

    var filteredElements = data;

    /*
    var filteredElements = _.filter(search_elements, function(e) { 
        return e.title.includes(toSearch) || e.content.includes(toSearch); 
    });
    */
    var temp = getPaginatedItems(filteredElements, page);

    var search_result = $("#search_results");
    var paginationElement = $("#paginationElement");
    var results_count = $("#results_count span");

    var noResults_message = $("#searchMessages #noResults");
    var sorry_message = $("#searchMessages #sorry");
    var show_message = $("#searchMessages #show");

    search_result.empty();
    paginationElement.empty();

    if (filteredElements.length == 0) {

        results_count.html(noResults_message.text() + ' "' + toSearch + '"');

        search_result.append(
            $('<div />', {
                class: 'show-for-medium medium-1 large-1 columns',
                text: ""
            }
            ).append("&nbsp;").add(
                $('<div />', {
                    class: 'small-12 medium-10 large-10 columns float-center'
                }
                ).append(
                    $('<div />', {
                        class: 'row',
                        html: sorry_message.html()
                    }
                    )
                )
            ).add(
                $('<div />', {
                    class: 'show-for-medium medium-1 large-1 columns'
                }
                )
            )
        )
    } else {
        paginationElement.append(
            createPagination(temp, toSearch)
        );

        results_count.html(show_message.text().split(",")[0] + ' 1-10 ' + show_message.text().split(",")[1] + ' ' + temp.total + show_message.text().split(",")[2] + ' "' + toSearch + '"');

        temp.data.forEach(function (element) {
            //var row = document.createElement("div");
            search_result.append(
                $('<div />', {
                    class: 'row'
                }
                ).append(
                    createSearchElement(element.title, element.content, element.url, toSearch, element.onClickEventTitle, element.onClickEventUrl)
                )
            );
        });
        window.scrollTo(0, 0);
    }
}

var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
};

function getPaginatedItems(items, page) {
    var page = page || 1,
        per_page = 10,
        offset = (page - 1) * per_page,
        paginatedItems = _.slice(items, offset).slice(0, per_page);
    return {
        page: page,
        per_page: per_page,
        total: items.length,
        total_pages: Math.ceil(items.length / per_page),
        data: paginatedItems
    };
}

$(function () {
    $("#searchicon").click(function (e) {
        e.preventDefault();
        $("#formsearch").submit();
    });

    $("#formsearch").on("submit", function (e) {
        var searchValue = $("#searchtext")[0].value;
        if (searchValue === "") {
            e.preventDefault();
            return false;
        }
        dataLayerPush(createGTMSearchEntryObject(searchValue));
    })

    $("#searchForm").on("submit", function (e) {
        var searchValue = $("#searchField")[0].value;
        if (searchValue != "") {
            e.preventDefault();
            doSearch(searchValue);
            dataLayerPush(createGTMSearchEntryObject(searchValue));

        } else {
            e.preventDefault();
            return false;
        }
    })

    var toSearch = getUrlParameter('searchtext');
    if (!(typeof toSearch === 'undefined' || toSearch === null)) {
        doSearch(toSearch);
    }

    $("#searchButton").click(function () {
        var searchValue = $("#searchField")[0].value;
        if (searchValue != "") {
            doSearch(searchValue);
            dataLayerPush(createGTMSearchEntryObject(searchValue));
        }
    });
});

function createGTMSearchEntryObject(value) {

    var gtmObj = {
        'event': 'search',
        'searchAction': 'search entry',
        'searchText': value
    }
    return gtmObj;
}