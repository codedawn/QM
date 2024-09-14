namespace QM
{
    public static class ResponseExtension
    {
        public static bool IsSuccess(this IResponse response)
        {
            return response.Code == (int)NetworkCode.Success;
        }
    }
}
