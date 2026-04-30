using System.Drawing;
using System.Windows.Forms;

namespace LibraryProject.Views
{
    public class Curation : Form
    {
        public Curation()
        {
            this.Text = "나를 위한 맞춤 추천";
            this.Size = new Size(700, 600);

            Label lblChart = new Label { Text = "나의 분야별 독서 분포", Location = new Point(30, 20), AutoSize = true, Font = new Font("Arial", 12, FontStyle.Bold) };

            // 차트 대용 영역 (실제 라이브러리 사용 전 레이아웃용)
            Panel pnlChartBase = new Panel { Location = new Point(30, 50), Size = new Size(620, 200), BorderStyle = BorderStyle.FixedSingle, BackColor = Color.GhostWhite };
            pnlChartBase.Controls.Add(new Label { Text = "[분야별 독서 차트]", Location = new Point(250, 90) });

            Label lblRecommend = new Label { Text = "AI 추천 도서 목록", Location = new Point(30, 270), AutoSize = true, Font = new Font("Arial", 12, FontStyle.Bold) };

            // 추천 도서 카드 레이아웃용 FlowLayoutPanel
            FlowLayoutPanel flowRecommended = new FlowLayoutPanel { Location = new Point(30, 300), Size = new Size(620, 250), AutoScroll = true };

            // 예시 도서 카드 추가 (UI 구현 예시)
            for (int i = 1; i <= 3; i++)
            {
                Button btnBook = new Button { Text = $"추천 도서 {i}\n(C# 프로그래밍)", Size = new Size(180, 100), Margin = new Padding(10) };
                flowRecommended.Controls.Add(btnBook);
            }

            this.Controls.AddRange(new Control[] { lblChart, pnlChartBase, lblRecommend, flowRecommended });
        }
    }
}