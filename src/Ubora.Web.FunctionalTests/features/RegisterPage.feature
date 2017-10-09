Feature: Register page
    As a user
    I want to click on all buttons on Register page and register an user

Background:
    Given I go to Home page
        And I clicked on the element "#SignInSignUp"
        And I clicked on the element "a=Sign up"

Scenario: I click on Logo
    When I click on the element ".header-logo"
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I click on Sign in/sign up
    When I click on the element "#SignInSignUp"
    Then I expect the title of the page "Sign in to UBORA - UBORA"

Scenario: I click on Terms of Service
    When I click on the element "a=Terms of Service"
    Then I expect the title of the page "Sign up - UBORA"

Scenario: I submit valid registration form then user is logged in and full name displayed
    When I click on the element "#SignInSignUp"
        And I sign up as "email@email.com"
        And I click on the element "span=Menu"
    Then I expect the element "p=TestFirstName TestLastName" is visible
    When I click on the element "#SignOut"
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I sumbit empty registration form and try to create an account
    When I click on the element "button=Create an account"
    Then I expect the element "span=The FirstName field is required." is visible
        And I expect the element "span=The LastName field is required." is visible
        And I expect the element "span=The Email field is required." is visible
        And I expect the element "span=The Password field is required." is visible
        And I expect the element "span=The Confirm password field is required." is visible
        And I expect the element "span=This field is required." is visible
        And I expect the title of the page "Sign up - UBORA"