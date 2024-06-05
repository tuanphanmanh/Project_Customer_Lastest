import { animate, state, style, transition, trigger } from '@angular/animations';

export function appModuleAnimation() {
    return slideFromBottom();
}

export function accountModuleAnimation() {
    return slideFromUp();
}

export function slideFromBottom() {
    return trigger('routerTransition', [
        state('void', style({ 'padding-top': '20px', opacity: '0' })),
        state('*', style({ 'padding-top': '0px', opacity: '1', 'position': 'relative' })),
        transition(':enter', [animate('0.33s ease-out', style({ opacity: '1', 'padding-top': '0px', 'position': 'relative' }))]),
    ]);
}

export function slideFromUp() {
    return trigger('routerTransition', [
        state('void', style({ 'margin-top': '-10px', opacity: '0' })),
        state('*', style({ 'margin-top': '0px', opacity: '1', 'position': 'relative' })),
        transition(':enter', [animate('0.2s ease-out', style({ opacity: '1', 'margin-top': '0px', 'position': 'relative' }))]),
    ]);
}
