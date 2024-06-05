using System;

namespace esign
{
    /// <summary>
    /// Some consts used in the application.
    /// </summary>
    public class AppConsts
    {
        /// <summary>
        /// Default page size for paged requests.
        /// </summary>
        public const int DefaultPageSize = 10;

        /// <summary>
        /// Maximum allowed page size for paged requests.
        /// </summary>
        public const int MaxPageSize = 1000;

        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public const string DefaultPassPhrase = "gsKxGZ012HLL3MI5";

        public const int ResizedMaxProfilePictureBytesUserFriendlyValue = 1024;

        public const int MaxProfilePictureBytesUserFriendlyValue = 5;

        public const string TokenValidityKey = "token_validity_key";
        public const string RefreshTokenValidityKey = "refresh_token_validity_key";
        public const string SecurityStampKey = "AspNet.Identity.SecurityStamp";

        public const string TokenType = "token_type";

        public static string UserIdentifier = "user_identifier";

        public const string ThemeDefault = "default";
        public const string Theme2 = "theme2";
        public const string Theme3 = "theme3";
        public const string Theme4 = "theme4";
        public const string Theme5 = "theme5";
        public const string Theme6 = "theme6";
        public const string Theme7 = "theme7";
        public const string Theme8 = "theme8";
        public const string Theme9 = "theme9";
        public const string Theme10 = "theme10";
        public const string Theme11 = "theme11";
        public const string Theme12 = "theme12";
        public const string Theme13 = "theme13";

        public static TimeSpan AccessTokenExpiration = TimeSpan.FromHours(8);
        public static TimeSpan RefreshTokenExpiration = TimeSpan.FromDays(365);

        public static string STATUS_DRAFT_CODE = "Draft";
        public static string STATUS_WAITING_CODE = "Waiting";
        public static string STATUS_ONPROGRESS_CODE = "OnProgress";
        public static string STATUS_COMPLETED_CODE = "Completed";
        public static string STATUS_REVOKED_CODE = "Revoked";

        public static int TYPE_STATUS_REQUEST_ID = 0;
        public static int TYPE_STATUS_SIGNER_ID = 1;

        public static string C_WWWROOT = "wwwroot";
        public static string C_UPLOAD_TEMP_FOLDER = "FileUpload";
        public static string C_UPLOAD_ORIGINAL_EXTENSION = ".original";
        public static string C_UPLOAD_VIEW_EXTENSION = ".view";
        public static string C_OPTION_EXPECTED_DATE = "OptionExpectedDate";

        public static string HISTORY_CODE_REJECTED = "Rejected";
        public static string HISTORY_CODE_SIGNATUREREQUEST = "SignatureRequest";
        public static string HISTORY_CODE_SHARED = "Shared";
        public static string HISTORY_CODE_SIGNED = "Signed";
        public static string HISTORY_CODE_SIGN_ERROR = "SignError";
        public static string HISTORY_CODE_CREATED = "Created";
        public static string HISTORY_CODE_REASSIGNED = "Reassigned";
        public static string HISTORY_CODE_CC = "Cc";
        public static string HISTORY_CODE_TRANSFERRED = "Transferred";
        public static string HISTORY_CODE_REMINDED = "Reminded";
        public static string HISTORY_CODE_REVOKE = "Revoked";
        public static string HISTORY_CODE_COMMENTED = "Commented";

        public static string EMAIL_CODE_WAIT = "WAIT";
        public static string EMAIL_CODE_COMPLETE = "COMPLETE";
        public static string EMAIL_CODE_SIGNED = "SIGNED"; // for requester
        public static string EMAIL_CODE_REJECT = "reject";

        public static string EMAIL_CODE_REVOKE = "REVOKE";
        public static string EMAIL_CODE_REASSIGN = "REASSIGN";
        public static string EMAIL_CODE_TRANSFER = "TRANSFER";
        public static string EMAIL_CODE_SHARE = "SHARE";
        public static string EMAIL_CODE_REMIND = "REMIND";
        public static string EMAIL_CODE_COMMENT = "COMMENT";
        public static string EMAIL_CODE_ADDEDREFERENCEFILE = "AddedReferenceFile";
        public static string EMAIL_CODE_ADDEDREFERENCEDOC = "AddedReferenceDoc";
        public static string EMAIL_CODE_AUTOREMINDER = "AUTOREMINDER";

        public static string TYPE_TEMPLATE_MOBILE_NOTIFICATION = "MOBILE_NOTIFICATION";

        public static string URL_CODE_SIGNED_OR_REJECTED = "SIGNED_OR_REJECTED";
        public static string URL_CODE_GET_TOKEN = "GET_TOKEN";
        public static string URL_CODE_REASSIGN = "REASSIGN";

        public static string STATUS_OTHER_SYSTEM_APPROVED = "APPROVED"; // for requester
        public static string STATUS_OTHER_SYSTEM_REJECTED = "REJECTED";

        public static string TYPE_PDF = ".PDF";

    }
}
