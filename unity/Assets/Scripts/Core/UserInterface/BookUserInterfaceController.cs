using Framework.Controller;
using ScriptableObjects.Book;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Core.UserInterface
{
    public class BookUserInterfaceController : BaseController<BookUserInterfaceController>
    {
        public RectTransform bookPanel;
        [SerializeField] private BookData debugBookData;
        [SerializeField] private TMP_Text leftSideText;
        [SerializeField] private TMP_Text rightSideText;
        [SerializeField] private Image leftSideImage;
        [SerializeField] private Image rightSideImage;
        [SerializeField] private TMP_Text leftPagination;
        [SerializeField] private TMP_Text rightPagination;
        [SerializeField] private Button previousButton;
        [SerializeField] private Button nextButton;

        private BookData currentBook;
        private int currentPageIndex = 0;

        public void Start()
        {
            if (debugBookData != null)
                SetBook(debugBookData);

            if (previousButton != null)
                previousButton.onClick.AddListener(PreviousPage);

            if (nextButton != null)
                nextButton.onClick.AddListener(NextPage);
        }

        public void SetBook(BookData bookData)
        {
            bookPanel.gameObject.SetActive(true);
            currentBook = bookData;
            currentPageIndex = 0;
            SetupContent();
        }

        private void SetupContent()
        {
            if (currentBook == null || currentBook.bookContent.Count == 0)
                return;

            BookPage page = currentBook.bookContent[currentPageIndex];

            leftSideText.text = page.contentLeft;
            rightSideText.text = page.contentRight;

            if (page.contentLeftImage != null)
            {
                leftSideImage.sprite = page.contentLeftImage;
                leftSideImage.gameObject.SetActive(true);
            }
            else
            {
                leftSideImage.gameObject.SetActive(false);
            }

            if (page.contentRightImage != null)
            {
                rightSideImage.sprite = page.contentRightImage;
                rightSideImage.gameObject.SetActive(true);
            }
            else
            {
                rightSideImage.gameObject.SetActive(false);
            }

            leftSideText.ForceMeshUpdate();
            rightSideText.ForceMeshUpdate();

            leftSideText.pageToDisplay = 1;
            rightSideText.pageToDisplay = 1;

            UpdatePagination();
            UpdateButtons();
        }

        private void UpdatePagination()
        {
            leftPagination.text = (currentPageIndex * 2 + 1).ToString();
            rightPagination.text = (currentPageIndex * 2 + 2).ToString();
        }

        private void UpdateButtons()
        {
            if (previousButton != null)
                previousButton.interactable = currentPageIndex > 0;

            if (nextButton != null)
                nextButton.interactable = currentBook != null && currentPageIndex + 1 < currentBook.bookContent.Count;
        }

        public void PreviousPage()
        {
            if (currentPageIndex > 0)
            {
                currentPageIndex--;
                SetupContent();
            }
        }

        public void NextPage()
        {
            if (currentBook != null && currentPageIndex + 1 < currentBook.bookContent.Count)
            {
                currentPageIndex++;
                SetupContent();
            }
        }
    }
}
