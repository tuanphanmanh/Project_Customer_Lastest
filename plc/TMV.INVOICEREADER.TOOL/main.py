import os, sys
import argparse
import uuid

os.system('pip3 install pytesseract opencv-python Pillow python-dotenv PyPDF2 cx_Oracle')
import pytesseract
import cv2
import pyodbc
import cx_Oracle
from PIL import Image
from dotenv import load_dotenv
from utils.PDFHelper import PDFHelper
from utils.DatabaseHelper import DatabaseHelper

if __name__ == '__main__':
    # load đường dẫn file pdf từ cmd
    parser = argparse.ArgumentParser()
    parser.add_argument("-f", "--filepath", required=True, help="Đường dẫn đến tệp ảnh")
    args = parser.parse_args()
    try:
        pdfPath = args.filepath
    except:
        print('Syntax error: use python yourscript.py -f "PDF_File_Path"')
    
    # load config
    load_dotenv('config.env')
    pytesseract.pytesseract.tesseract_cmd = os.getenv('TESSERACT_PATH')

    try:
        dsn = cx_Oracle.makedsn(os.getenv('DATABASE_SERVER'), os.getenv('DATABASE_PORT'), service_name=os.getenv('DATABASE_SERVICE_NAME'))
        conn = cx_Oracle.connect(user=os.getenv('DATABASE_USERNAME'), password=os.getenv('DATABASE_PASSWORD'), dsn=dsn)
    except Exception as e:
        print(str(e))
        sys.exit(0)

    # tạo uuid 
    uuid = str(uuid.uuid4())

    # lấy các ảnh trong file PDF
    img_arr = []
    try:
        img_arr = PDFHelper.PDF2Images(pdfPath)
    except Exception as e: 
        print(str(e))
        sys.exit(0)


    # # duyệt xử lý từng segment
    for i in range(len(img_arr)): # duyệt từ 0->số trang
        page = PDFHelper.identifyPage(img_arr[i])
        print(f"Page {i}: Type {page['type']}")

        if (page['type'] == 1):
            data = PDFHelper.parseSegmentA(page['data'], img_arr[i])
            DatabaseHelper.InsertSegmentA(conn, data=data, uuid=uuid)

        if (page['type'] == 2):
            data = PDFHelper.parseSegmentB(page['data'], img_arr[i])
            for invoice in data:
                DatabaseHelper.InsertSegmentB(conn, data=invoice, uuid=uuid)

        if (page['type'] == 3):
            data = PDFHelper.parseSegmentC(page['data'])
            for invoice in data:
                DatabaseHelper.InsertSegmentC(conn, data=invoice, uuid=uuid)

        # if (page['type'] == 4):
        #     data = PDFHelper.parseSegmentD(page['data'], img_arr[i])
        #     for packing in data:
        #         DatabaseHelper.InsertSegmentD(conn, data=packing, uuid=uuid)

        if (page['type'] == 5):
            data = PDFHelper.parseSegmentE(page['data'])
            for packingAttached in data:
                DatabaseHelper.InsertSegmentE(conn, data=packingAttached, uuid=uuid)


    DatabaseHelper.MergeToTempTable(conn, uuid)
    DatabaseHelper.ProcessTempData(conn)