import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LocalStorageService } from '@shared/utils/local-storage.service';
import { PdfViewerComponent } from '@syncfusion/ej2-angular-pdfviewer';
import { ConfirmSendRequestComponent } from '../esign-modal/confirm-send-request/confirm-send-request.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';

@Component({
    selector: 'app-preview',
    templateUrl: './preview.component.html',
    styleUrls: ['./preview.component.less'],
    animations: [appModuleAnimation()],
})
export class PreviewComponent extends AppComponentBase implements OnInit {
    @ViewChild('confirmRequest') confirmRequest!: ConfirmSendRequestComponent;
    @ViewChild('pdfviewer') public pdfviewerControl: PdfViewerComponent;

    public document: string = 'http://esign-standby.toyotavn.com.vn:5001/Document/BC_%C4%90ATN_PhiVanMinh_140762.pdf';

    documentData: any;

    data: any = {
        title: 'Form Designer',
        message: 'The Form Designer application showcases the Form Designer component, which is used to design and generate PDF documents interactively.',
        category: 'Ringi, Frame Contract',
        projectScheduleFrom: '2019-05',
        projectScheduleTo: '2020-05',
        content: 'The Form Designer application showcases the Form Designer component, which is used to design and generate PDF documents interactively. It allows you to create, edit, and save the PDF document in the designer interface',
        totalCost: '1000000',
        roi: '1.5',
        documents: [
            {
                name: 'Form Designer',
                fileNum: 1,
                page: 6,
                signature: 'https://ej2.syncfusion.com/demos/src/pdfviewer/images/signature/signature1.png',
                signers: [
                    {
                        name: 'Nancy Davolio',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    },
                    {
                        name: 'Andrew Fuller',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    },
                    {
                        name: 'Janet Leverling',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    },
                    {
                        name: 'Margaret Peacock',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    },
                ]
            },
            {
                name: 'Form Designer',
                page: 6,
                fileNum: 2,
                signature: 'https://ej2.syncfusion.com/demos/src/pdfviewer/images/signature/signature1.png',
                signers: [
                    {
                        name: 'Nancy Davolio',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    },
                    {
                        name: 'Andrew Fuller',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    },
                    {
                        name: 'Janet Leverling',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    },
                    {
                        name: 'Margaret Peacock',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    },
                ]
            },
            {
                name: 'Form Designer',
                page: 6,
                fileNum: 3,
                signature: 'https://ej2.syncfusion.com/demos/src/pdfviewer/images/signature/signature1.png',
                signers: [
                    {
                        name: 'Nancy Davolio',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    },
                    {
                        name: 'Andrew Fuller',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    },
                    {
                        name: 'Janet Leverling',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    },
                    {
                        name: 'Margaret Peacock',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    },
                ]
            },
            {
                name: 'Form Designer',
                page: 6,
                fileNum: 4,
                signature: 'https://ej2.syncfusion.com/demos/src/pdfviewer/images/signature/signature1.png',
                signers: [
                    {
                        name: 'Nancy Davolio',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    },
                    {
                        name: 'Andrew Fuller',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    },
                    {
                        name: 'Janet Leverling',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    },
                    {
                        name: 'Margaret Peacock',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    },
                ]
            },
            {
                name: 'Form Designer',
                page: 6,
                fileNum: 5,
                signature: 'https://ej2.syncfusion.com/demos/src/pdfviewer/images/signature/signature1.png',
                signers: [
                    {
                        name: 'Nancy Davolio',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    },
                    {
                        name: 'Andrew Fuller',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    },
                    {
                        name: 'Janet Leverling',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    },
                    {
                        name: 'Margaret Peacock',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    },
                ]
            },
            {
                name: 'Form Designer',
                page: 6,
                fileNum: 6,
                signature: 'https://ej2.syncfusion.com/demos/src/pdfviewer/images/signature/signature1.png',
                signers: [
                    {
                        name: 'Nancy Davolio',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    },
                    {
                        name: 'Andrew Fuller',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    },
                    {
                        name: 'Janet Leverling',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    },
                    {
                        name: 'Margaret Peacock',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    },
                ]
            },
        ],
        signers: [
            {
                rank: 1,
                signers: [
                    {
                        name: 'Nancy Davolio',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                        signature: 6,
                        color: '#e7552c',
                    },
                    {
                        name: 'Andrew Fuller',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                        signature: 6,
                        color: '#e7552c',

                    },
                    {
                        name: 'Janet Leverling',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                        signature: 6,
                        color: '#e7552c',
                    },
                    {
                        name: 'Margaret Peacock',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                        signature: 6,
                        color: '#e7552c',
                    },
                ]
            },
            {
                rank: 2,
                signers: [
                    {
                        name: 'Nancy Davolio',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                        signature: 6,
                        color: '#e7552c',
                    },

                ]
            },
            {
                rank: 3,
                signers: [
                    {
                        name: 'Nancy Davolio',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                        signature: 6,
                        color: '#e7552c',
                    },
                ],
            },
            {
                rank: 4,
                signers: [
                    {
                        name: 'Nancy Davolio',
                        imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                        signature: 6,
                        color: '#e7552c',
                    },
                ],
            },
        ],
    };
    signerData = [
        {
            rank: 1,
            signers: [
                {
                    name: 'John Doe1',
                    email: 'JohnDoe1@toyotavn.com.vn',
                    imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    color: '#7265e6',
                    backgroundColor: '#e6e6ff',
                    numOfSign: 2,
                    check: true
                },
                {
                    name: 'John Doe2',
                    email: 'JohnDoe2@toyotavn.com.vn',
                    imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    color: '#4dcdaa',
                    backgroundColor: '#e6fff2',
                    numOfSign: 1,
                    check: true
                },
            ]
        },
        {
            rank: 2,
            signers: [
                {
                    name: 'John Doe3',
                    email: 'JohnDoe3@toyotavn.com.vn',
                    imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    color: '#e9bd3f',
                    backgroundColor: '#fff2e6',
                    numOfSign: 1,
                    check: false
                },

            ]
        },
        {
            rank: 3,
            signers: [
                {
                    name: 'John Doe4',
                    email: 'JohnDoe4@toyotavn.com.vn',
                    imgUrl: 'https://i.pinimg.com/564x/89/8c/e8/898ce8537c9d8192175319d9c48cb029.jpg',
                    color: '#dc60eb',
                    backgroundColor: '#ffe6ff',
                    numOfSign: 1,
                    check: false
                },

            ]
        },
    ];
    constructor(injector: Injector,private local : LocalStorageService) {
        super(injector);
    }

    ngOnInit() {
        this.local.getItem('documentData',(err,data)=>{
            this.documentData = data
        })


    }

    pdfDoneLoading(params){
        this.documentData.formFields.forEach(e => {
            this.pdfviewerControl.formDesignerModule.addFormField(e.type, Object.assign({},e) as any);
        })
    }

}
