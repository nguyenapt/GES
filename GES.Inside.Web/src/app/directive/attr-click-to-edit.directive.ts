import { Directive, ElementRef } from '@angular/core';

@Directive({
    selector: '[attr-click-to-edit]'
})
export class AttrClickToEditDirective {

    constructor(Element: ElementRef) {

    }
}
