namespace UI
{
    public interface IView
    {
        /// <summary>
        /// View의 이벤트를 ViewModel에 연결
        /// </summary>
        void BindViewToViewModel();

        /// <summary>
        /// ViewModel의 데이터 변경을 View에 반영
        /// </summary>
        void BindViewModelToView();
        
        void UnbindAll();
    }
}