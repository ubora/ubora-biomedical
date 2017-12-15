export class Autocomplete {
  static initialize() {
    window.addEventListener('DOMContentLoaded', event => {
        const endpoint = this._getApiEndpoint('input[name="searchUsersUrl"]');
        const targetElement = '.js-search-user';

        this._createAutoComplete(endpoint, targetElement);
    });
  }

  _getApiEndpoint(nodeSelector) {
    const node = document.querySelector(nodeSelector);
    if (node !== null && node.value !== '') {
      return node.value;
    } else {
      throw Error('Invalid API endpoint or none given.');
    }
  }

  _createAutoComplete(apiEndpoint, targetElement) {
    return AutoComplete({
      Limit: 10,
      MinChars: 2,
      Url: apiEndpoint,
      QueryArg: "searchPhrase",
      EmptyMessage: "No results.",
      _RenderResponseItems: function (response) {
        let limit = this._Limit();

        if (limit < 0) {
          response = response.reverse();
        } else if (limit === 0) {
          limit = response.length;
        }

        const resultList = document.createElement('ul');
        resultList.classList.add('autocomplete-result-list');

        for (let item = 0; item < Math.min(Math.abs(limit), response.length); item++) {
          const resultElement = document.createElement('li');
          resultElement.classList.add('autocomplete-result');
          resultElement.textContent = `${response[item].Label} – ${response[item].Value}`;
          resultElement.setAttribute("data-autocomplete-value", response[item].Value);
          resultList.appendChild(resultElement);
        }

        return resultList;
      },
      _Highlight: function (label) {
        return label;
      },
    }, targetElement);
  }
}

const autocompleteElement = document.querySelector('.js-search-user');
if (autocompleteElement) {
    Autocomplete.initialize();
}
