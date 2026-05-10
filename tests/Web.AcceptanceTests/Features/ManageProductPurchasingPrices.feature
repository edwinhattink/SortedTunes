Feature: Manage Product Purchasing Prices

Managing product purchasing prices

@User
@PrepareProduct
Scenario: Can add historical product price
    Given the user navigates to 'cost-models/products'
    And they search for prepared product and open the product details
    When the user adds the historical purchasing price for price 3, quantity 2 and date "02/01/2024"
    Then the purchasing price table has a row with unit price 3, quantity 2 and date "02/01/2024"

@User
@PrepareProduct
Scenario: Can delete a purchasing price row
    Given the user navigates to 'cost-models/products'
    And they search for prepared product and open the product details
    When the user adds the historical purchasing price for price 4.20, quantity 5 and date "03/01/2024"
    And they delete the purchasing price with unit price 4.20, quantity 5 and date "03/01/2023"
    Then the purchasing price table does not have a row with unit price 4.20, quantity 5 and date "03/01/2023"

@User
@PrepareProduct
Scenario: Can edit a purchasing price row
    Given the user navigates to 'cost-models/products'
    And they search for prepared product and open the product details
    When the user adds the historical purchasing price for price 5, quantity 3 and date "04/01/2023"
    And they edit the historical purchasing price to price 6, quantity 4 and date "05/01/2023"
    Then the purchasing price table has a row with unit price 6, quantity 4 and date "05/01/2023"