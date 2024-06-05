<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TMV.Barcode.Default" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="icon" type="image/x-icon" href="img/favicon.ico" />
    <title>Barcode Scanning</title>
    <link type="text/css" rel="stylesheet" href="css/style.css" />
    <script type="text/javascript" src="js/jquery-1.11.1.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            $("#txtusername").focus();
            // Always focus on Scan Textbox
            $(document).click(function () { $("#txtusername").focus(); });
            // Add shortcut key - keydown in Scan Textbox
            $('#txtusername').keydown(function (event) {
                var keycode = (event.keyCode ? event.keyCode : event.which);
                if (keycode == '49' && event.shiftKey) {
                    //SHIFT+1: LOGOUT
                    event.preventDefault();
                    btnLogout.click();
                }                
            });
        });

        function PlayDoneSound() {
            document.getElementById('done_sound').play();
        }

        function PlayErrSound() {
            document.getElementById('error_sound').play();
        }
    </script>
</head>
<body>       
    <embed type="audio/mpeg" id="error_sound" autoplay="false" src="Audio/error.mp3" hidden="TRUE" height="0" width="0" />
    <div class="body_overlay">
        <div class="scanning_form">
            <div class="block_scanning">
                <div class="warning" visible="false" id="dError" runat="server">
                    <span class="red"><b>*Thông báo:</b></span>
                    <label id="errorText" runat="server">User không tồn tại!</label>
                </div>

                <form class="form" id="frmforgotpass" name="frmforgotpass" action=""  defaultbutton="btnLogin" runat="server"  novalidate="novalidate" autocomplete="on">
                    <div class="title-bar opMode-icon">Barcode Login</div>
                    <div class="info-box pd">
                        <div class="guide tr">Thông tin đăng nhập</div>
                        <div class="tr">
                            <label for="txtusername" class="w144"><span class="red">*</span>User</label>
                            <asp:TextBox ID="txtusername" runat="server" type="text" class="input"></asp:TextBox>
                        </div>
                        <div class="button-tr tr mg144" align ="center">
                             <asp:Button ID="btnLogin" runat="server" Text="Login" class="btn btn-blue btn70" OnClick="btnLogin_Click" />                             
                        </div>
                    </div>                               
                    <!--/ info-box -->                    
                </form>
                
            </div>
            <!--/ block_scanning -->
        </div>
    </div>
    <asp:Literal ID="_StartJS" runat="server" />
</body>
</html>
