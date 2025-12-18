// Simple autocomplete component
function initAutocomplete(inputId, hiddenInputId, searchUrl, displayText = null) {
    const input = document.getElementById(inputId);
    const hiddenInput = document.getElementById(hiddenInputId);
    let dropdown = null;
    let selectedItem = null;
    let searchTimeout = null;

    if (!input) return;

    // Create dropdown container
    function createDropdown() {
        if (dropdown) return dropdown;
        dropdown = document.createElement('div');
        dropdown.className = 'autocomplete-dropdown';
        dropdown.style.cssText = 'position: absolute; z-index: 1000; background: white; border: 1px solid #ddd; border-radius: 4px; max-height: 200px; overflow-y: auto; width: 100%; box-shadow: 0 2px 8px rgba(0,0,0,0.1); display: none;';
        input.parentElement.style.position = 'relative';
        input.parentElement.appendChild(dropdown);
        return dropdown;
    }

    function showDropdown() {
        if (!dropdown) createDropdown();
        dropdown.style.display = 'block';
    }

    function hideDropdown() {
        if (dropdown) dropdown.style.display = 'none';
    }

    function renderSuggestions(suggestions) {
        if (!dropdown) createDropdown();
        dropdown.innerHTML = '';
        
        if (suggestions.length === 0) {
            dropdown.innerHTML = '<div class="p-2 text-muted">No results found</div>';
            showDropdown();
            return;
        }

        suggestions.forEach(item => {
            const option = document.createElement('div');
            option.className = 'autocomplete-option p-2';
            option.style.cssText = 'cursor: pointer; padding: 8px; border-bottom: 1px solid #eee;';
            option.textContent = item.text;
            option.addEventListener('mouseenter', () => {
                option.style.backgroundColor = '#f0f0f0';
            });
            option.addEventListener('mouseleave', () => {
                option.style.backgroundColor = 'white';
            });
            option.addEventListener('click', () => {
                selectItem(item);
            });
            dropdown.appendChild(option);
        });
        showDropdown();
    }

    function selectItem(item) {
        selectedItem = item;
        input.value = item.text;
        if (hiddenInput) {
            hiddenInput.value = item.id;
        }
        hideDropdown();
        input.dispatchEvent(new Event('change', { bubbles: true }));
    }

    function search(query) {
        if (query.length < 2) {
            hideDropdown();
            return;
        }

        clearTimeout(searchTimeout);
        searchTimeout = setTimeout(() => {
            fetch(`${searchUrl}?search=${encodeURIComponent(query)}&limit=10`)
                .then(response => response.json())
                .then(data => {
                    renderSuggestions(data);
                })
                .catch(error => {
                    console.error('Autocomplete error:', error);
                    hideDropdown();
                });
        }, 300);
    }

    // Event listeners
    input.addEventListener('input', function() {
        const query = this.value.trim();
        if (!query) {
            if (hiddenInput) hiddenInput.value = '';
            selectedItem = null;
            hideDropdown();
            return;
        }
        
        // If the input matches the selected item, don't search
        if (selectedItem && selectedItem.text === query) {
            return;
        }
        
        search(query);
    });

    input.addEventListener('focus', function() {
        const query = this.value.trim();
        if (query.length >= 2) {
            search(query);
        }
    });

    input.addEventListener('blur', function() {
        // Delay to allow click events on dropdown
        setTimeout(() => {
            hideDropdown();
        }, 200);
    });

    // Set initial value if displayText is provided
    if (displayText && hiddenInput && hiddenInput.value) {
        input.value = displayText;
        selectedItem = { id: hiddenInput.value, text: displayText };
    }
}

