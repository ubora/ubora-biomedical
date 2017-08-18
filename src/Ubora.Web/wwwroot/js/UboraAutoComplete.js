function autoComplete(searchUsersUrl) {
    AutoComplete({
        Limit: 100,
        MinChars: 2,
        Url: searchUsersUrl,
        QueryArg: "searchPhrase",
        EmptyMessage: "No results",
        _RenderResponseItems: function (response) {
            var ul = document.createElement("ul"),
                li = document.createElement("li"),
                limit = this._Limit();

            // Order
            if (limit < 0) {
                response = response.reverse();
            } else if (limit === 0) {
                limit = response.length;
            }

            for (var item = 0; item < Math.min(Math.abs(limit), response.length); item++) {
                li.innerHTML = response[item].Label + ' - ' + response[item].Value;
                li.setAttribute("data-autocomplete-value", response[item].Value);

                ul.appendChild(li);
                li = document.createElement("li");
            }

            return ul;
        },
    }, "#autocomplete");
}