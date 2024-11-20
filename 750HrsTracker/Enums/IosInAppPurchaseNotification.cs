namespace _750HrsTracker.Enums
{
    public enum AppleNotificationTypes
    {
        SUBSCRIBED,
        DID_CHANGE_RENEWAL_PREF,
        DID_CHANGE_RENEWAL_STATUS,
        OFFER_REDEEMED,
        DID_RENEW,
        EXPIRED,
        DID_FAIL_TO_RENEW,
        GRACE_PERIOD_EXPIRED,
        PRICE_INCREASE,
        REFUND,
        REFUND_DECLINED,
        CONSUMPTION_REQUEST,
        RENEWAL_EXTENDED,
        REVOKE,
        TEST,
        RENEWAL_EXTENSION,
        REFUND_REVERSED,
        EXTERNAL_PURCHASE_TOKEN,
        ONE_TIME_CHARGE,
    }
    
    public enum AppleNotificationSubTypes
    {
        INITIAL_BUY,
        RESUBSCRIBE,
        DOWNGRADE,
        UPGRADE,
        AUTO_RENEW_ENABLED,
        AUTO_RENEW_DISABLED,
        VOLUNTARY,
        BILLING_RETRY,
        PRICE_INCREASE,
        GRACE_PERIOD,
        PENDING,
        ACCEPTED,
        BILLING_RECOVERY,
        PRODUCT_NOT_FOR_SALE,
        SUMMARY,
        FAILURE,
        UNREPORTED,
    }

    public enum AppleNotificationDataEnvironmentName
    {

    }
}
