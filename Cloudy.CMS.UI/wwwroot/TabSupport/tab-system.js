﻿import Button from '../button.js';

class TabSystem {
    constructor() {
        this.element = document.createElement('poetry-ui-tab-system');
        this.tabs = document.createElement('poetry-ui-tabs');
        this.element.append(this.tabs);
        this.content = document.createElement('poetry-ui-tab-panel');
        this.element.append(this.content);
    }

    addTab(text, content) {
        var tab = document.createElement('poetry-ui-tab');
        tab.innerText = text;
        tab.tabIndex = 0;

        tab.addEventListener("keyup", event => {
            if (event.keyCode != 13) {
                return;
            }

            event.preventDefault();
            tab.click();
        });

        tab.addEventListener('click', () => {
            [...this.tabs.children].forEach(c => c.classList.remove('poetry-ui-active'));
            tab.classList.add('poetry-ui-active');

            [...this.content.children].forEach(c => this.content.removeChild(c));
            this.content.append(content());

            tab.blur();
        });

        if (this.tabs.childElementCount == 0) {
            tab.click();
        }

        this.tabs.append(tab);

        return this;
    }

    appendTo(element) {
        element.appendChild(this.element);

        return this;
    }
}

export default TabSystem;