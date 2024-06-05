from datetime import datetime

def parseDate(strDate: str):
    return datetime.strptime(strDate, "%b. %d, %Y")

def formatNumber(strNumber: str):
    return float(strNumber.replace(',', ''))