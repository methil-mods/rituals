using Framework.Controller;
using ScriptableObjects.Book;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Core.UserInterface
{
    public class BookUserInterfaceController : BaseController<BookUserInterfaceController>
    {
        [Header("UI References")]
        [SerializeField] private RectTransform bookPanel;
        [SerializeField] private TMP_Text leftSideText;
        [SerializeField] private TMP_Text rightSideText;
        [SerializeField] private Image leftSideImage;
        [SerializeField] private Image rightSideImage;
        [SerializeField] private TMP_Text leftPagination;
        [SerializeField] private TMP_Text rightPagination;
        [SerializeField] private Button previousButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button quitButton;

        private BookData currentBook;
        private int currentPageIndex;

        public UnityAction<BookData> OnBookOpened;
        public UnityAction<BookData> OnBookClosed;
        public UnityAction<BookPage, BookPage, BookData> OnPageChanged;

        private void Start()
        {
            previousButton?.onClick.AddListener(PreviousPage);
            nextButton?.onClick.AddListener(NextPage);
            quitButton?.onClick.AddListener(CloseBook);
            bookPanel.gameObject.SetActive(false);
        }

         public void OpenBook(BookData bookData) 
         {
            if (bookData == null || bookData.bookContent.Count == 0)
                return;

            currentBook = bookData;
            currentPageIndex = 0;
            bookPanel.gameObject.SetActive(true);
            UpdatePage();
            OnBookOpened?.Invoke(bookData);
        }

        public void CloseBook()
        {
            bookPanel.gameObject.SetActive(false);
            OnBookClosed?.Invoke(currentBook);
        }

        private void UpdatePage()
        {
            var page = currentBook.bookContent[currentPageIndex];

            SetPageContent(leftSideText, leftSideImage, page.contentLeft, page.contentLeftImage);
            SetPageContent(rightSideText, rightSideImage, page.contentRight, page.contentRightImage);

            UpdatePagination();
            UpdateButtons();
        }

        private void SetPageContent(TMP_Text textField, Image imageField, string text, Sprite image)
        {
            textField.text = text ?? string.Empty;
            textField.pageToDisplay = 1;
            textField.ForceMeshUpdate();

            bool hasImage = image != null;
            imageField.gameObject.SetActive(hasImage);
            if (hasImage)
                imageField.sprite = image;
        }

        private void UpdatePagination()
        {
            leftPagination.text = (currentPageIndex * 2 + 1).ToString();
            rightPagination.text = (currentPageIndex * 2 + 2).ToString();
        }

        private void UpdateButtons()
        {
            if (currentBook == null) return;

            previousButton.interactable = currentPageIndex > 0;
            nextButton.interactable = currentPageIndex + 1 < currentBook.bookContent.Count;
        }

        public void PreviousPage()
        {
            if (currentPageIndex <= 0) return;

            var lastPage = currentBook.bookContent[currentPageIndex];
            currentPageIndex--;
            var newPage = currentBook.bookContent[currentPageIndex];

            UpdatePage();
            OnPageChanged?.Invoke(lastPage, newPage, currentBook);
        }

        public void NextPage()
        {
            if (currentBook == null || currentPageIndex + 1 >= currentBook.bookContent.Count)
                return;

            var lastPage = currentBook.bookContent[currentPageIndex];
            currentPageIndex++;
            var newPage = currentBook.bookContent[currentPageIndex];

            UpdatePage();
            OnPageChanged?.Invoke(lastPage, newPage, currentBook);
        }
    }
}
