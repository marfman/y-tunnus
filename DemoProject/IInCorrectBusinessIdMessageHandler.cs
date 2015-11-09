namespace DemoProject
{
    /// <summary>
    /// 
    /// </summary>
    public interface IInCorrectBusinessIdMessageHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inCorrectBusinessIdCase"></param>
        /// <returns></returns>
        string GetErrorMessage(InCorrectBusinessIdCase inCorrectBusinessIdCase);
    }
}
