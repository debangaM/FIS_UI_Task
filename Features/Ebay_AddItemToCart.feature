Feature: Ebay_AddItemToCart
This feature will test the add to cart functionality

Background: 
	Given I navigate to url 'https://www.ebay.com/'

@positiveflow
Scenario: Search for a book, add it to cart and then validate if the book has been added to cart
	Then I am on the page titled 'Electronics, Cars, Fashion, Collectibles & More | eBay'
	Given I load all elements of "EbayPageObjectLocators"
	When I type "book" into "search_textbox"
	When I click on "search_button"
	When I click on "search_result" and focus on new tab
	When I click on "addToCart_button"
	Then The page contains "cartItemsCountis1_text"
	Then The page contains "cartItemsCountis1_numeric"
	When I close current page and focus on previous tab
	Then I am on the page titled 'Book for sale | eBay'

@negativeflow
Scenario: Failure scenario to generate screenshot on failure
	Then I am on the page titled 'abc'