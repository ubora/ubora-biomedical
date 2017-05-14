Feature: Register page
    As a developer
    I want the demo app have correct page

Background:
    Given I go to the website "/Account/Register"

Scenario: Get the title of Ubora 
    Then I expect the title of the page "Sign up - UBORA"

Scenario: Add firstname only to a input field and entered then empty forms displayed that field is required
    When I click on the element "#IsAgreedToTermsOfService"
    When I set "TestFirstName" to the element "#FirstName"
    When I click on the element "/html/body/main/section/form/button"
    Then I expect that should be "The LastName field is required." the text of the page "span=The LastName field is required."
    Then I expect that should be "The Email field is required." the text of the page "span=The Email field is required."
    Then I expect that should be "The Password field is required." the text of the page "span=The Password field is required."
    Then I expect that should be "The Confirm password field is required." the text of the page "span=The Confirm password field is required."

Scenario: I submits valid registration form then user is logged in and full name displayed
    When I click on the element "#IsAgreedToTermsOfService"
    When I set "TestFirstName" to the element "#FirstName"
    When I set "TestLastName" to the element "#LastName"
    When I set "email4@email.com" to the element "#Email"
    When I set "Test12345" to the element "#Password"
    When I set "Test12345" to the element "#ConfirmPassword"
    When I click on the element "/html/body/main/section/form/button"
    Then I expect the title of the page "Welcome - UBORA"
    When I click on the element "/html/body/header/button"
    Then I expect that the element "p=TestFirstName TestLastName" is visible



    



