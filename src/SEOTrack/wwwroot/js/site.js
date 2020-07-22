// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function SearchResult(data) {
    this.rankNum = ko.observable(data.rankNum);
}

function SearchGoogleViewModel() {
    var self = this;
    self.searchResults = ko.observableArray([]);
    self.keywords = ko.observable();
    self.searchUrl = ko.observable();
    self.error = ko.observable(false);
    self.noResults = ko.observable(false);
    
    self.searchGoogle = function () {
        $('#loading').removeClass('hide');
        self.searchResults([]);
        self.error(false);
        self.noResults(false);
        $.ajax("/Home/RankingResults", {
            type: "post",
            contentType: "application/json",
            data: ko.toJSON({
                "keywords": self.keywords,
                "url": self.searchUrl
            }),
            success: function (data, textstatus, jqXHR) {
                if (jqXHR.status === 204) {
                    self.noResults(true);
                }
                else {
                    data.forEach(item => self.searchResults.push(new SearchResult({ rankNum: item.rankNum })));
                }
            },
            error: function () {
                self.error(true);
            },
            complete: function () {
                $('#loading').addClass('hide');
            }
        });
        
    };
}

ko.applyBindings(new SearchGoogleViewModel());