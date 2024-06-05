import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';

export class GlobalValidator {
    static greaterThan0AndLessThan999999(control: FormControl) {
        const FLOAT_POSITIVE_NUMBER = /^(?:[1-9]\d*|0)?(?:\.\d{0,2})?$/g;
        if (control && control.value && (!FLOAT_POSITIVE_NUMBER.test(control.value.toString().trim()) ||
            parseFloat(control.value.toString().trim()) < 0 ||
            parseFloat(control.value.toString().trim()) > 999999)) {
            return { greaterThan0AndLessThan999999: true };
        }
        return null;
    }

    static maxLength(max) {
        return (control: FormControl) => {
            if (control && control.value) {
                if (control.value.toString().trim().length > max) {
                    return { maxLength: true };
                }
            }
            return null;
        };
    }

    static fixedLength(length) {
        return (control: FormControl) => {
            if (control && control.value) {
                if (control.value.toString().trim().length !== length) {
                    return { fixedLength: true };
                }
            }
            return null;
        };
    }

    static requiredPassword(control: AbstractControl) {

        // if (Validators.required(control)) {
        //   return {requiredPassword: true};
        // }
        // if (control.value.toString().trim().length === 0) {
        //   return {requiredPassword: true};
        // }

        const required = /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$/g;
        if (control && control.value && !required.test(control.value)) {
            return { requiredPassword: true };
        }
        return null;
    }

    static minLength(min) {
        return (control: FormControl) => {
            if (control && control.value) {
                if (control.value.toString().trim().length < min) {
                    return { minLength: true };
                }
            }
            return null;
        };
    }

    static inputFormat(control: FormControl) {
        //  /^[A-Za-z0-9 _]*[A-Za-z0-9][A-Za-z0-9 _]*$/;
        // /^[0-9A-Za-z\s\-_]+$/
        const INPUT_REGEXP = /^[0-9A-Za-z]+$/;
        if (control && control.value && !INPUT_REGEXP.test(control.value)) {
            return {
                validateInputText: {
                    valid: false,
                    message: 'Not valid input.'
                }
            };
        }
        return null;
    }

    static emailFormat(control: FormControl) {
        const EMAIL_REGEX = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/gm;
        if (control && control.value && !EMAIL_REGEX.test(control.value.toString().trim())) {
            return {
                emailFormat: true
            };
        }
        return null;
    }

    static phoneFormat(control: FormControl) {
        const PHONE_REGEX = /^(0|\+84)[0-9]{9,14}$/gm; // BUG 16561,16904
        if (control && control.value && !PHONE_REGEX.test(control.value.toString().trim().replace(/\s/g, ''))) {
            return {
                phoneFormat: true
            };
        }
        return null;
    }

    static taxFormat(control: FormControl) {
        const TAX_REGEX = /^[0-9\-]{10,13}$/g;
        if (control && control.value && !TAX_REGEX.test(control.value.toString().trim())) {
            return {
                taxFormat: true
            };
        }
        return null;
    }

    static peopleIdFormat(control: FormControl) {
        if (control && control.value && control.value.toString().trim().length !== 9 && control.value.toString().trim().length !== 12) {
            return {
                peopleIdFormat: true
            };
        }
        return null;
    }

    static today(control: FormControl) {
        if (control && control.value) {
            const date = new Date(control.value);
            const today = new Date();
            if (date.getMonth() === today.getMonth() && date.getDate() === today.getDate() && date.getFullYear() === today.getFullYear()) {
                return null;
            }
            return {
                today: true
            };
        }
    }

    static lessThanToday(control: FormControl) {
        if (control && control.value) {
            const date = new Date(control.value);
            const today = new Date();
            if (date < today) {
                return {
                    lessThanToday: true
                };
            }
            return null;
        }
    }

    static numberFormat(control: FormControl) {
        const NUMBER_REGEX = /^([0-9]*)$/g;
        if (control && control.value) {
            if (typeof control.value === 'string') {
                const num = control.value.toString().trim().replace(/,([0-9]{3})/g, '$1');
                if (!NUMBER_REGEX.test(num)) {
                    return {
                        numberFormat: true
                    };
                }
            } else {
                if (!NUMBER_REGEX.test(control.value)) {
                    return {
                        numberFormat: true
                    };
                }
            }
            // if (parseInt(control.value.toString().trim()) === 0) {
            //   return {
            //     'numberFormat': true
            //   }
            // }
        }
        return null;
    }

    static numberFormatGreaterThanZero(control: FormControl) {
        const NUMBER_REGEX = /^([0-9]*)$/g;
        if (control && control.value && (!NUMBER_REGEX.test(control.value.toString().trim()) || parseFloat(control.value.toString().trim()) === 0)) {
            return { numberFormatGreaterThanZero: true };
        }
        return null;
    }

    static numberPercent(control: FormControl) {
        const NUMBER_REGEX = /^\d*\.?\d*$/;
        if (control && control.value) {
            if (typeof control.value === 'string') {
                const num = control.value.toString().trim().replace(/,([0-9]{3})/g, '$1');
                if (!NUMBER_REGEX.test(num)) {
                    return { numberPercent: true };
                }
            } else {
                if (!NUMBER_REGEX.test(control.value)) {
                    return { numberPercent: true };
                }
            }
            if (Number(control.value) < 0 || Number(control.value) > 100) {
                return { numberPercent: true };
            }
        }
        return null;
    }

    static positiveAndNegInteger(control: FormControl) {
        const POS_N_NEG_INT = /^([+-]?[1-9]\d*|0)$/g;
        if (control && control.value) {
            if (typeof control.value === 'string') {
                const num = control.value.toString().trim().replace(/,/g, '');
                if (!POS_N_NEG_INT.test(num)) {
                    return {
                        posAndNegInt: true
                    };
                }
            } else {
                if (!POS_N_NEG_INT.test(control.value)) {
                    return {
                        posAndNegInt: true
                    };
                }
            }
        }
        return null;
    }

    static floatNumberFormat(control: FormControl) {
        const FLOAT_POSITIVE_NUMBER = /^(?:[1-9]\d*|0)?(?:\.\d{0,2})?$/g;
        if (control && control.value && (!FLOAT_POSITIVE_NUMBER.test(control.value.toString().trim()) || parseFloat(control.value.toString().trim()) === 0)) {
            return { floatNumberFormat: true };
        }
        return null;
    }

    static floatNumber3Format(control: FormControl) {
        const FLOAT_POSITIVE_NUMBER = /^(?:[1-9]\d*|0)?(?:\.\d{0,3})?$/g;
        if (control && control.value && (!FLOAT_POSITIVE_NUMBER.test(control.value.toString().trim()) || parseFloat(control.value.toString().trim()) === 0)) {
            return { floatNumberFormat: true };
        }
        return null;
    }

    static floatNumberFormatHourl(control: FormControl) {
        const FLOAT_POSITIVE_NUMBER = /^(?:[1-9]\d*|0)?(?:\.\d{0,1})?$/g;
        if (control && control.value && (!FLOAT_POSITIVE_NUMBER.test(control.value.toString().trim()) || parseFloat(control.value.toString().trim()) === 0)) {
            return { floatNumberFormatHourl: true };
        }
        return null;
    }

    static floatNumberFormatHourlNext(control: FormControl) {
        const FLOAT_POSITIVE_NUMBER = /^(?:[1-9]\d*|0)?(?:\.\d{0,1})?$/g;
        if (control && control.value && (!FLOAT_POSITIVE_NUMBER.test(control.value.toString().trim()))) {
            return { floatNumberFormatHourl: true };
        }
        return null;
    }

    static floatNumberFormatHourl3(control: FormControl) {
        const FLOAT_POSITIVE_NUMBER = /^(?:[1-9]\d*|0)?(?:\.\d{0,1})?$/g;
        if (control && control.value && parseFloat(control.value.toString().trim()) > 3) {
            return { floatNumberFormatHourl3: true };
        }
        return null;
    }

    static floatNumberFormat0(control: FormControl) {
        const FLOAT_NUMBER_ACCEPT_ZERO = /^(?:[1-9]\d*|0)?(?:\.\d{0,2})?$/g;
        if (control && control.value && !FLOAT_NUMBER_ACCEPT_ZERO.test(control.value.toString().trim())) {
            return { floatNumberFormat0: true };
        }
        return null;
    }

    static squareNumberValidate(control: FormControl) {
        const SQUARE_NUMBER_VALIDATE = /^([1-9]\d{0,11})?(?:\.\d{1,2})?$/g;
        if (control && control.value && !SQUARE_NUMBER_VALIDATE.test(control.value.toString().trim())) {
            return { squareNumberValidate: true };
        }
        return null;
    }

    static petrolQuantityAmount(control: FormControl) {
        const PETROL_QUANTITY_NUMBER_VALIDATE = /^([1-9]\d{0,9})?(?:\.\d{1,2})?$/g;
        if (control && control.value && !PETROL_QUANTITY_NUMBER_VALIDATE.test(control.value.toString().trim())) {
            return { petrolQuantityAmount: true };
        }
        return null;
    }

    static required(control: FormControl) {
        if (Validators.required(control)) {
            return { required: true };
        }
        if (control.value.toString().trim().length === 0) {
            return { required: true };
        }
        return null;
    }

    static blank(control: FormControl) {
        if (control.value && control.value.toString().trim().length === 0) {
            return { blank: true };
        }
        return null;
    }

    static futureDate(control: FormControl) {
        if (control && control.value) {
            const date = new Date(control.value.toString().trim());
            date.setHours(0, 0, 0, 0);
            const today = new Date();
            today.setHours(0, 0, 0, 0);
            if (date > today) {
                return { futureDate: true };
            }
        }
        return null;
    }

    static dateFormat(control: FormControl) {
        const DATE_REGEX = /^(0[1-9]|[12][0-9]|3[01]|[1-9])[- /.](0[1-9]|1[012]|[1-9])[- /.]((19|20)\d{2})$/;
        if (control && control.value && !DATE_REGEX.test(control.value.toString().trim())) {
            return { dateFormat: true };
        }
        const dateMatch = control.value.toString().trim().match(DATE_REGEX);
        if (!(dateMatch[1] === new Date(dateMatch[2] + '/' + dateMatch[1] + '/' + dateMatch[3]).getDate())) {
            return { dateFormat: true };
        }
        return null;
    }

    static justNormalString(control: FormControl) {
        const LETTER_NUMBER_REGEX = /^[0-9A-Za-z]*$/g;
        if (control && control.value && !LETTER_NUMBER_REGEX.test(control.value.toString().trim())) {
            return { justNormalString: true };
        }
        return null;
    }

    static specialCharacter(control: FormControl) {
        const LETTER_NUMBER_REGEX = /[/$&+,"{}[\]`:;=?@#~|</\\/>*()%!^`]/g;
        // const LETTER_NUMBER_REGEX = /\W/g;
        if (control && control.value && LETTER_NUMBER_REGEX.test(control.value.toString().trim())) {
            return { specialCharacter: true };
        }
        return null;
    }

    static registerNo(control: FormControl) {
        // const LETTER_NUMBER_REGEX = /^[0-9]{2,3}[A-Za-z]{1,2}[0-9]?[-][0-9]{3,4}[.]?[0-9]{0,2}$/g;
        const LETTER_NUMBER_REGEX = /^(\d{2})?-?[A-Za-z]{1,2}\d?-?\d{2,3}-?\.?\d{0,2}$/im;
        if (control && control.value && !LETTER_NUMBER_REGEX.test(control.value.toString().trim())) {
            return { registerNo: true };
        }
        return null;
    }

    static bothFieldFalse(field1, field2) {
        return (form: FormGroup) => {
            const v1 = form.value[field1];
            const v2 = form.value[field2];

            const result = !v1 && !v2 ? { bothFieldFalse: [field1, field2] } : undefined;
            if (result) {
                form.get(field2).setErrors({ bothFieldFalse: true });
            }
            return result;
        };
    }

    static bothThreeFieldFalse(field1, field2, field3) {
        return (form: FormGroup) => {
            const v1 = form.value[field1];
            const v2 = form.value[field2];
            const v3 = form.value[field3];

            const result = !v1 && !v2 && !v3 ? { bothThreeFieldFalse: [field1, field2, field3] } : undefined;
            if (result) {
                form.get(field3).setErrors({ bothThreeFieldFalse: true });
            }
            return result;
        };
    }

    static requiredAll(...args) {
        return (form: FormGroup) => {
            const errField = [];
            for (let i = 0; i < args.length; i++) {
                if (form.controls[args[i]].value === null || form.controls[args[i]].value === '') {
                    errField.push(args[i]);
                    form.controls[args[i]].setErrors({ requiredAll: true });
                }
            }
            return errField.length ? { requiredAll: errField } : undefined;
        };
    }

    static dateMore(field, acceptAqual?) {
        return (control: FormControl) => {
            if (control && control.value) {
                const group = control.parent as FormGroup;
                const compControl = group.controls[field];
                if (compControl && compControl.value && control.value < compControl.value) {
                    return { dateMore: true };
                }
                if (!acceptAqual && control.value < compControl.value) {
                    return { dateMore: true };
                }
            }
            return null;

        };
    }

    static dateLess(field, acceptAqual?) {
        return (control: FormControl) => {
            if (control && control.value) {
                const group = control.parent as FormGroup;
                const compControl = group.controls[field];
                if (compControl && compControl.value && control.value > compControl.value) {
                    return { dateLess: true };
                }
                if (!acceptAqual && control.value < compControl.value) {
                    return { dateLess: true };
                }
            }
            return null;

        };
    }

    static requiredOnField(field) {
        return (control: FormControl) => {
            const group = control.parent as FormGroup;
            if (group) {
                const fieldControl = group.controls[field];
                if (fieldControl && fieldControl.value && control && !control.value) {
                    return { required: true };
                }
            }
            return null;
        };
    }

    static telFormat(control: FormControl) {
        const TEL_REGEX = /^0[0-9]{6,15}$/g;
        if (control && control.value && !TEL_REGEX.test(control.value.toString().trim().replace(/\s/g, ''))) {
            return {
                telFormat: true
            };
        }
        return null;
    }

    static depositDate(control: FormControl) {
        if (control && control.value) {
            const date = new Date(control.value).getDate();
            const month = new Date(control.value).getMonth();
            const year = new Date(control.value).getFullYear();
            const today = new Date();
            if (month === today.getMonth() && date === today.getDate() && year === today.getFullYear()) {
                return null;
            }
            return {
                depositDate: true
            };
        }
    }

    static changeDeliveryDlr(control: FormControl) {
        if (control && control.value) {
            const date = new Date(control.value).getDate();
            const month = new Date(control.value).getMonth();
            const year = new Date(control.value).getFullYear();

            const today = new Date();
            if (month === today.getMonth() && date === today.getDate() && year === today.getFullYear()) {
                return null;
            }
            return {
                changeDeliveryDlr: true
            };
        }
    }

    static estimatedDate(control: FormControl) {
        if (control && control.value) {
            const date = new Date(control.value.toString().trim()).getTime();
            const today = new Date().getTime();

            if (date < today) {
                return {
                    estimatedDate: true
                };
            }
            return null;
        }
    }

    static numberFormatAcceptZero(control: FormControl) {
        const NUMBER_REGEX = /^([0-9]*)$/g;
        if (control && control.value) {
            if (typeof control.value === 'string') {
                const num = control.value.toString().trim().replace(/,([0-9]{3})/g, '$1');
                if (!NUMBER_REGEX.test(num)) {
                    return {
                        numberFormatAcceptZero: true
                    };
                }
            } else {
                if (!NUMBER_REGEX.test(control.value)) {
                    return {
                        numberFormatAcceptZero: true
                    };
                }
            }
            if (control.value.toString().trim().length === 0) {
                return {
                    numberFormatAcceptZero: true
                };
            }
        }
        return null;
    }

    static maxLengthUserName(control: FormControl) {
        if (control.value != undefined && control.value.toString().trim().length > 47) {
            return { maxLengthUserName: true };
        }
        return null;
    }
}

