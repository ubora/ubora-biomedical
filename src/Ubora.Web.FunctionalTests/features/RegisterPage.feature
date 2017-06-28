Feature: Register page
    As a developer
    I want the demo app to have correct page

Background:
    Given I go to the website "/Home/Index"
    Given I click the element "a=Sign in/sign up"
    Given I click the element "a=Sign up"

Scenario: Get the title of Ubora 
    Then I expect the title of the page "Sign up - UBORA"

Scenario: I click on Logo
    When I click on the element ".header-logo"
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I click on Sign in/sign up
    When I click on the element "a=Sign in/sign up"
    Then I expect the title of the page "Sign in to UBORA - UBORA"

Scenario: I click on Terms of Service
    When I click on the element "a=Terms of Service"
    Then I expect the title of the page "- UBORA"

Scenario: I submit valid registration form then user is logged in and full name displayed
    When I click on the element "#IsAgreedToTermsOfService"
    When I set "TestFirstName" to the element "#FirstName"
    When I set "TestLastName" to the element "#LastName"
    When I set "email@email.com" to the element "#Email"
    When I set "Test12345" to the element "#Password"
    When I set "Test12345" to the element "#ConfirmPassword"
    When I click on the element "button=Create an account"
    Then I expect the title of the page "- UBORA"
    When I click on the element "span=Menu"
    Then I expect the element "p=TestFirstName TestLastName" is visible