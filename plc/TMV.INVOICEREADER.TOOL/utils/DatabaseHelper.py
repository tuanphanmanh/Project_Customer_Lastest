import cx_Oracle
import re
import os, sys
from DTO.Part import Part
from DTO.PackingAttached import PackingAttached
from DTO.OriginalPackingList import OriginalPackingList
from DTO.OriginalInvoice import OriginalInvoice
from DTO.FlightInfo import FlightInfo
from utils.Utils import formatNumber, parseDate

class DatabaseHelper():
    def __init__():
        super().__init__()

    @staticmethod
    def InsertSegmentA(conn, data: FlightInfo, uuid: str):
        cursor = conn.cursor()
        match = re.findall(r"([0-9\-]+)", data.MAWB)
        data.MAWB = match[0]
        cursor.callproc('pkg_rpa_jsp.InsertSegmentA', 
                        [data.ShipmentNo,
                         data.ShipperNameAndAddress, data.ConsigneesNameAndAddress, data.IssuedBy,
                         data.IssuingCarriersAgent, data.MAWB, data.AirportOfDeparture,
                         data.FlightTo, data.ByFirstCarrier, data.Currency,
                         data.DeclareValueForCarria, data.AirportOfDestination, data.RequestedFlightDate, uuid
                        ])
        cursor.close()

    
    @staticmethod
    def InsertSegmentB(conn, data: OriginalInvoice, uuid: str):
        cursor = conn.cursor()
        cursor.callproc('pkg_rpa_jsp.InsertSegmentB', 
                        [data.InvoiceNo, parseDate(data.InvoiceDate), data.SoldToMessis,
                         data.SpecialInstructions, data.FlightFrom, data.FlightTo,
                         data.Vessel, parseDate(data.SailingOnOfAbout), data.PaymentTerms,
                         data.DescriptionOfGoods, formatNumber(data.Qty), formatNumber(data.Fob),
                         formatNumber(data.Freight), formatNumber(data.Insurance), uuid
                        ])
        cursor.close()

    @staticmethod
    def InsertSegmentC(conn, data: Part, uuid: str):
        cursor = conn.cursor()
        cursor.callproc('pkg_rpa_jsp.InsertSegmentC', 
                        [data.InvoiceNo, parseDate(data.InvoiceDate), data.Page,
                         data.LotNo, data.PartNo, data.DescriptionOfGoods,
                         formatNumber(data.Qty), formatNumber(data.UnitPrice), formatNumber(data.FobAmount),
                         formatNumber(data.NetWeight), data.Org, uuid
                        ])
        cursor.close() 

    # @staticmethod
    # def InsertSegmentD(conn, data: OriginalPackingList, uuid: str):
    #     cursor = conn.cursor()
    #     cursor.callproc('pkg_rpa_jsp.InsertSegmentD', 
    #                     [data.InvoiceNo, parseDate(data.InvoiceDate), data.Vessel,
    #                      parseDate(data.SailingOnOfAbout), data.SoldToMessis, data.FlightFrom,
    #                      data.FlightTo, data.DescriptionOfGoods, formatNumber(data.Qty),
    #                      data.M3, formatNumber(data.NetWeight), formatNumber(data.GrossWeight), uuid
    #                     ])
    #     cursor.close()

    @staticmethod
    def InsertSegmentE(conn, data: PackingAttached, uuid: str):
        cursor = conn.cursor()
        cursor.callproc('pkg_rpa_jsp.InsertSegmentE', 
                        [data.InvoiceNo, parseDate(data.InvoiceDate), data.Page,
                         data.LotNo, data.InvCase, formatNumber(data.Qty),
                         formatNumber(data.NetWeight), formatNumber(data.GrossWeight), data.MMent,
                         data.MaterialType, parseDate(data.ETD), uuid
                        ])
        cursor.close()

    @staticmethod
    def MergeToTempTable(conn, uuid: str):
        cursor = conn.cursor()
        cursor.callproc('pkg_rpa_jsp.MergeToTempTable', 
                        [ uuid ])
        cursor.close()

    @staticmethod
    def ProcessTempData(conn):
        cursor = conn.cursor()
        cursor.callproc('pkg_temp_jsp_invoice.jsp_invoice_process', 
                        [ ])
        cursor.close()