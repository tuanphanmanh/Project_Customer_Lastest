import pytesseract
import cv2
import os
import re
import numpy as np
from DTO.Part import Part
from DTO.PackingAttached import PackingAttached
from DTO.OriginalPackingList import OriginalPackingList
from DTO.OriginalInvoice import OriginalInvoice
from DTO.FlightInfo import FlightInfo
from Constant.Posititions import *
from Constant.IdentifyKey import *
from PyPDF2 import PdfReader
from PIL import Image
from io import BytesIO

class PDFHelper:
    def __init__():
        super().__init__()

    @staticmethod
    def PDF2Images(pdfPath: str):
        """
        Convert PDF file to images array
        :param pdfPath: Path to PDF file.
        """ 
        data = []
        reader = PdfReader(pdfPath)
        for page in range(len(reader.pages)):
            pdf_page = reader.pages[page]
            xObject = pdf_page['/Resources']['/XObject'].get_object()
            for obj in xObject:
                if xObject[obj]['/Subtype'] == '/Image':
                    image_data = xObject[obj].get_data()
                    image = Image.open(BytesIO(image_data))
                    data.append(image)

        return data
    
    @staticmethod
    def identifyPage(image: Image):
        """
        Nhận dạng 1 page. Convert ảnh đó ra text từ đó nhận dạng.
        Return:
        data: The extracted text\n
        type: 
        - 1: SegmentA
        - 2: SegmentB
        - 3: SegmentC
        - 4: SegmentD
        - 5: SegmentE
        
        """
        lang = os.getenv('OCR_LANG')
        config = os.getenv('OCR_CONFIG')
        extractedInformation: str = pytesseract.image_to_string(image, lang = lang, config = config)
        if (extractedInformation.find(SEGMENTA_KEY) != -1):
            return {
                'type': 1,
                'data': extractedInformation
            }
        if (extractedInformation.find(SEGMENTB_KEY) != -1):
            return {
                'type': 2,
                'data': extractedInformation
            }
        if (extractedInformation.find(SEGMENTD_KEY) != -1):
            return {
                'type': 4,
                'data': extractedInformation
            }
        else:
            # nếu không phải thì thử xoay ảnh
            degree = -90  # Góc xoay -90 độ
            rotated_image = image.rotate(degree, expand=True)
            extractedInformation = pytesseract.image_to_string(rotated_image, lang = lang, config = config)
            if (extractedInformation.find(SEGMENTC_KEY) != -1):
                return {
                    'type': 3,
                    'data': extractedInformation
                }
            if (extractedInformation.find(SEGMENTE_KEY) != -1):
                return {
                    'type': 5,
                    'data': extractedInformation
                }
            

            return {
                'type': 0,
                'data': None
            }

    @staticmethod
    def getTextByPosition(image: Image, pos: list):
        lang = 'eng' 
        config = r'--oem 3 --psm 6'
        img = image.crop((pos[0][0], pos[0][1], pos[1][0], pos[1][1]))
        invoiceNo: str = pytesseract.image_to_string(img, lang = lang, config = config)
        img.save('test.png')
        return invoiceNo.strip().replace('\n', ' ')

    @staticmethod
    def parseSegmentA(data: str, image: Image):
        result: FlightInfo = FlightInfo()
        # match shipmentno

        pattern = r"[A-Z]+ - ([0-9]+)"
        match = re.findall(pattern, data)

        setattr(result, 'ShipmentNo', match[0])
        for k, v in SegmentAFields.items():
            setattr(result, k, PDFHelper.getTextByPosition(image, v))
        return result

    @staticmethod
    def parseSegmentB(data: str, image: Image):
        result = []

        pattern = r"([A-Z]{3}\. [0-9]{2}, [0-9]{4}) (.*?) ([0-9]+)"
        match = re.findall(pattern, data)

        invoiceDate = match[0][0]
        invoiceNo = match[0][1]

        textByPosition = {}
        for k, v in SegmentBFields.items():
            textByPosition[k] = PDFHelper.getTextByPosition(image, v)

        pattern = r" DOLLARS\n([A-Z ]+) ([0-9,\.]+).+\nFOB ([0-9,\.]+).+\nFREIGHT ([0-9,\.]+).+\nINSURANCE PREM\. ([0-9,\.]+)"
        match = re.findall(pattern, data)

        for i in match:
            invoice = OriginalInvoice()
            setattr(invoice, 'InvoiceNo', invoiceNo)
            setattr(invoice, 'InvoiceDate', invoiceDate)
            setattr(invoice, 'DescriptionOfGoods', i[0])
            setattr(invoice, 'Qty', i[1])
            setattr(invoice, 'Fob', i[2])
            setattr(invoice, 'Freight', i[3])
            setattr(invoice, 'Insurance', i[4])
            for k, v in textByPosition.items():
                setattr(invoice, k, v)
            
            result.append(invoice)

        return result

    @staticmethod
    def parseSegmentC(data: str):
        # remove space
        data = data.replace(' .', '.')

        # parse segment info
        pattern = r"([A-Z]{3}\. [0-9]{2}, [0-9]{4}) (.*?) ([0-9]+)"
        match = re.findall(pattern, data)

        invoiceDate = match[0][0]
        invoiceNo = match[0][1]
        invoicePage = match[0][2]

        # parse part list 
        result = []
        lotnos = re.findall(r"TMV[0-9]{3}", data)
        splitStr = re.split(r"TMV[0-9]{3}", data)
        for i in range(len(lotnos)):
            pattern = r"([0-9]{12}).*?[ ,|(]([A-Z, ]+)([0-9,\.]+) ([0-9,\.]+) ([0-9,\.]+) ([0-9,\.]+)"
            match = re.findall(pattern, splitStr[i+1])
            for part in match:
                invoicePart = Part()
                setattr(invoicePart, 'InvoiceNo', invoiceNo)
                setattr(invoicePart, 'InvoiceDate', invoiceDate)
                setattr(invoicePart, 'Page', invoicePage)
                setattr(invoicePart, 'LotNo', lotnos[i])
                setattr(invoicePart, 'PartNo', part[0])
                setattr(invoicePart, 'DescriptionOfGoods', part[1])
                setattr(invoicePart, 'Qty', part[2])
                setattr(invoicePart, 'UnitPrice', part[3])
                setattr(invoicePart, 'FobAmount', part[4])
                setattr(invoicePart, 'NetWeight', part[5])
                result.append(invoicePart)
                
        return result

    @staticmethod
    def parseSegmentD(data: str, image: Image):
        result = []
        # get data

        textByPosition = {}
        for k, v in SegmentDFields.items():
            textByPosition[k] = PDFHelper.getTextByPosition(image, v)

        pattern = r"NET GROSS\n([A-Z ]+) ([0-9,\.]+).*?([0-9,\.]+) ([0-9,\.]+) ([0-9,\.]+)"
        match = re.findall(pattern, data)

        for i in match:
            packing = OriginalPackingList()
            setattr(packing, 'DescriptionOfGoods', i[0])
            setattr(packing, 'Qty', i[1])
            setattr(packing, 'M3', i[2])
            setattr(packing, 'NetWeight', i[3])
            setattr(packing, 'GrossWeight', i[4])

            for k, v in textByPosition.items():
                setattr(packing, k, v)

            result.append(packing)
            
        return result
    
    @staticmethod
    def parseSegmentE(data: str):
        data = data.replace(' .', '.')

        # parse segment info
        pattern = r"([A-Z]{3}\. [0-9]{2}, [0-9]{4}) (.*?) ([0-9]+)"
        match = re.findall(pattern, data)

        invoiceDate = match[0][0]
        invoiceNo = match[0][1]
        invoicePage = match[0][2]

        pattern = r"(TMV[ 0-9]+).*?([A-Z0-9]+) ([0-9,\.]+) ([0-9,\.]+) ([0-9,\.]+) ([0-9,\.]+) "
        match = re.findall(pattern, data)
        result = []

        ## match ETD
        pattern = r"DATE:([A-Z]{3}\. [0-9]{2}, [0-9]{4})"
        ETD = re.findall(pattern, data)[0]

        for packing in match:
            packingAttached = PackingAttached()
            setattr(packingAttached, 'InvoiceNo', invoiceNo)
            setattr(packingAttached, 'InvoiceDate', invoiceDate)
            setattr(packingAttached, 'Page', invoicePage)
            setattr(packingAttached, 'LotNo', packing[0].replace(' ', ''))
            setattr(packingAttached, 'InvCase', packing[1])
            setattr(packingAttached, 'Qty', packing[2])
            setattr(packingAttached, 'NetWeight', packing[3])
            setattr(packingAttached, 'GrossWeight', packing[4])
            setattr(packingAttached, 'MMent', packing[5])
            setattr(packingAttached, 'ETD', ETD)
            result.append(packingAttached)

        return result