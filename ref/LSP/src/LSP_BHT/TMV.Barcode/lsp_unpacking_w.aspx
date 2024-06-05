<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="lsp_unpacking_w.aspx.cs" Inherits="TMV.Barcode.lsp_unpacking_w" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="icon" type="image/x-icon" href="img/favicon.ico" />
    <title>LSP U/P Scanning</title>
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

        function ConfirmDialog_FINISH() {
            if (confirm('Ban muon Ket thuc?')) {
                //True .. do something
                document.getElementById("txtScanValue").value = 'TMV_CONTE_FN_OK';
                document.getElementById("btnScan").click();
            }
        }

        function ConfirmDialog_ADHOC_KANBAN() {
            if (confirm('CONTENT da du hang, muon them Kanban nay?')) {
                //True .. do something
                document.getElementById("txtScanValue").value = 'TMV_KANBAN_AH_OK';
                document.getElementById("btnScan").click();
            }
        }

        function PlayDoneSound() {
            //document.getElementById('done_sound').play();
        }

        function PlayErrSound() {
            //document.getElementById('error_sound').play();
        }

        function LogOut() {
            //document.getElementById('done_sound').play();
        }
        
    </script>
</head>
<body>
    <!--
    <embed type="audio/mpeg" id="done_sound" autoplay="false" src="Audio/Done.mp3" hidden="TRUE" height="0" width="0" />   
    <embed type="audio/mpeg" id="error_sound" autoplay="false" src="Audio/error.mp3" hidden="TRUE" height="0" width="0" />
    -->
    <div class="body_overlay">
        <div class="scanning_form">
            <div class="block_scanning">
                <div class="warning" id="divError" runat="server" visible="false">
                    <span class="red"><b>*Thông báo:</b></span>
                    <label id="errorText" style="color: red; font-size: 14px" runat="server">không tồn tại!</label>
                </div>
                <form class="form" id="frm_scanning" runat="server" method="post" name="frm_scanning"
                    defaultbutton="btnScan" action="" novalidate="novalidate" autocomplete="on">
                    <!--title_area -->
                    <div class="title-bar opMode-icon">
                        <label>LSP</label>
                        <div style="clear: both; display: inline">
                        </div>
                        <asp:Label runat="server" ID="lblUserName" CssClass="userName"></asp:Label>
                        <div style="clear: both; display: inline"></div>
                        <asp:LinkButton ID="btnLogout" OnClientClick="return LogOut()" runat="server" OnClick="btnLogout_Click">Logout</asp:LinkButton>

                    </div>
                    <div class="info-box pd">
                        <!--info:BEGIN-->
                        <div id="info_area">
                            <div class="info_bar">
                                <div class="lbl_info" id="div_CONTENT" style="width: 75px; display: inline-table;">Content.:</div>
                                <div class="lbl_data" id="lbl_CONTENT" runat="server" align ="left"></div>                              
                                <div class="clear"></div>
                            </div>

                            <div class="info_bar">
                                <div class="lbl_info" id="div_Qty" style="width: 75px; display: inline-table;">No of Box:</div>
                                <div class="lbl_data" id="lbl_BOX_QTY" runat="server" align ="left"></div>                              
                                <div class="clear"></div>
                            </div>

                            <div class="info_bar" id ="div_PART" runat="server" visible="true">
                                <div class="lbl_info" style="width: 75px; display: inline-table;">PART No.:</div>
                                <div class="lbl_data" id="lbl_PART" runat="server" align ="left"></div>
                                <div class="clear"></div>
                           </div>  
                                                                                                                       
                      </div>
                      <!--info:END-->
                       
                        <div class="guide tr" style=" margin-top: 20px">
                            Scan thông tin barcode
                        </div>
                        <div class="tr">
                            <span class="red">*</span>
                            <label for="txtScanValue" id="lblScanCODE" class="w144" runat="server">
                                Value</label>&nbsp;&nbsp;
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
