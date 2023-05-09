using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public enum ToolbarEnum
    {
        Refresh,
        Search,
        Save,
        Update,
        Delete,
        Print,
        Etc1,
        Etc2,
        Etc3
    }
    // * Event 
    // 1. Define a delegate
    // 2. Define an event based on that delegate
    // 3. Raise the event 
    public delegate void MyBtnDelegate(object sender, string[] trueBtnNames);
    public delegate void MyMsgDelegate(object sender, string message);

    /// <summary>
    /// 메인화면에서 개발폼에서 구현된 IMyToolbar를 캐스팅해서 사용
    /// 1. 툴바버튼 클릭시 FormFunction 실행
    /// 2. NotifyBtnChange Event: 툴바 버튼 enable, disable
    /// 3. ValidBtnNames: 메인화면의 탭 페이지가 변경될 때 
    ///                   해당변수로 툴바 버튼 enable, disable
    /// 4. MyMsgDelegate Event: 메인화면 Status message로 사용
    /// Function 명: Refresh, Select, Insert, Update, Delete,  Print, .
    /// </summary>
    public interface IMainToolbar
    {
        // 해당 폼의 유효한 툴바 버튼
        string[] ValidBtnNames { get; set; }

        // 해당 Event발생 시 툴바 버튼 조정
        event MyBtnDelegate NotifyBtnChange;

        // 해당 Event발생 시 상태바 메시지 표시
        event MyMsgDelegate NotifyMsgChange;

        // 개발폼이나 사용자 컨트롤의 함수, 매개변수 툴바버튼 enum
        void FormFunction(ToolbarEnum toolbar);

    }
}
