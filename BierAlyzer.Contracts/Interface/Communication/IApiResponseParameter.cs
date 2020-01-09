using BierAlyzer.Contracts.Model;

namespace BierAlyzer.Contracts.Interface.Communication
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Interface for API response parameter. </summary>
    /// <remarks>   Andre Beging, 10.11.2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public interface IApiResponseParameter
    {
        RequestResult Result { get; set; }
    }
}
