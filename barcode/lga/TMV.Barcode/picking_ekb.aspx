<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="picking_ekb.aspx.cs" Inherits="TMV.Barcode.picking_v2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="icon" type="image/x-icon" href="img/favicon.ico" />
    <title>Picking Scanning</title>
    <link type="text/css" rel="stylesheet" href="css/style.css" />

    <script type="text/javascript" src="js/jquery-1.11.1.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtScanValue").focus();
            // Always focus on Scan Textbox
            $(document).click(function () { $("#txtScanValue").focus(); });
            // Add shortcut key - keydown in Scan Textbox
            $('#txtScanValue').keydown(function (event) {
                var keycode = (event.keyCode ? event.keyCode : event.which);
                if (keycode == '49' && event.shiftKey) {
                    //SHIFT+1: LOGOUT
                    event.preventDefault();
                    btnLogout.click();
                }                
            });
        });       

        function PlayDoneSound() {
            //document.getElementById('done_sound').play();
        }

        function PlayErrSound() {
            //document.getElementById('error_sound').play();
        }

        function LogOut() {
        }
       
    </script>
</head>
<body>
     <!--
    <embed   type="audio/mpeg" id="done_sound"  autoplay="false" src="Audio/Done.mp3"  hidden="TRUE" height="0" width="0" enablejavascript="true" />     
    <embed   type="audio/mpeg" id="error_sound" autoplay="false" src="Audio/error.mp3" hidden="TRUE" height="0" width="0" enablejavascript="true" />
    -->
    <div class="body_overlay">
        <div class="scanning_form">
            <div class="block_scanning">
                <div class="warning" id="divError" runat="server" visible="false">
                    <span class="red"><b>*Thông báo:</b></span>
                    <label id="errorText" style="color: red; font-size: 14px" runat="server"></label>
                </div>
                <form class="form" id="frm_scanning" runat="server" method="post" name="frm_scanning"
                    defaultbutton="btnScan" action="" novalidate="novalidate" autocomplete="on">
                    <!--title_area -->
                    <div class="title-bar opMode-icon">
                        <label>PCS PICKING</label>
                        <div style="clear: both; display: inline"></div>
                        <asp:Label runat="server" ID="lblUserName" CssClass="userName"></asp:Label>
                        <div style="clear: both; display: inline"></div>
                        <asp:LinkButton ID="btnLogout" OnClientClick="return LogOut()" runat="server" OnClick="btnLogout_Click">Logout</asp:LinkButton>

                    </div>
                    <div class="info-box pd">
                        <!--info:BEGIN-->
                        <div id="info_area">
                            <div class="info_bar">
                                <div class="lbl_info" id="divSCAN" style="width:20px; display: inline-table;">Scan:</div>
                                <div class="lbl_data" id="lbl_SCAN" runat="server" align ="left"></div>                              
                                <div class="clear"></div>
                            </div>
                                                                                                                                                           
                      </div>
                    <!--info:END-->
                       
                        <div class="guide tr" style=" margin-top: 20px">
                            Scan thông tin barcode
                        </div>
                        <div class="tr">
                            <span class="red">*</span>
                            <label for="txtScanValue" id="lblScanCODE" class="w144" runat="server">Value.</label>
                            <input name="txtScanValue" id="txtScanValue" type="text" value="" class="input" runat="server" style="text-transform: uppercase" />
                        </div>
                        <div class="button-tr tr mg144" align ="center">
                            <asp:Button ID="btnScan" runat="server" Text="Scan" class="btn btn-blue btn70" OnClick="btnScan_Click" />                            
                            <asp:HiddenField runat="server" ID="hdnTestValue" />                            
                        </div>                        
                    </div>
                    <input id="Hidden1" type="hidden" />
                </form>
            </div>
            <!--/ block_scanning -->
        </div>
    </div>
    <asp:Literal ID="_StartJS" runat="server" />
</body>
</html>
