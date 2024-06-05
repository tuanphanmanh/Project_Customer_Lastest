using System;
using System.Collections.Generic;
using System.Text;

namespace esign
{
    public enum TypeSignature
    {
        TYPE_SIGNATURE = 1,
        TYPE_NAME = 2,
        TYPE_TITLE = 3,
        TYPE_DATE_SIGNED = 4,
        TYPE_TEXT = 5,
        TYPE_COMPANY = 6,
    }

    public enum TypeSign
    {
        TYPE_SIGN_TEMPLATE = 1,
        TYPE_SIGN_DRAW = 2,
        TYPE_SIGN_UPLOAD = 3
    }

    public enum ActivityHistoryCode
    {
        Rejected,
        SignatureRequest,
        Shared,
        Signed,
        Reassigned,
        Cc,
        Transferred,
        AdditionalFile
    }

    public enum TransferType
    {
        All = 0,//all
        Me = 1,// from me
        Others = 2//to me
    }

    public enum MultiAffiliateActionCode
    {
        Submit,
        Sign,
        Reject,
        Comment,
        Reassign,
        Remind,
        Revoke,
        Transfer,
        Share,
        AdditionFile
    }
}
