Feature: Register page
    As a developer
    I want the demo app have correct page

Background:
    Given I go to the website "/Account/Register"

Scenario: Get the title of Ubora 
    Then I expect the title of the page "Sign up - UBORA"

Scenario: I submits valid registration form then user is logged in and full name displayed
    When I click on the element "#IsAgreedToTermsOfService"
    When I set "TestFirstName" to the element "#FirstName"
    When I set "TestLastName" to the element "#LastName"
    When I set "email@email.com" to the element "#Email"
    When I set "Test12345" to the element "#Password"
    When I set "Test12345" to the element "#ConfirmPassword"
    When I click on the element "/html/body/main/section/form/button"
    Then I expect the title of the page "- UBORA"
    When I click on the element "/html/body/header/button"
    Then I expect that the element "p=TestFirstName TestLastName" is visible