Feature: Register page
    As a user
    I want to click on all buttons on Register page and register an user

Background:
    Given I go to Home page
        And I clicked on the element "span=Log in"
        And I clicked on the element "a=Sign up"

Scenario: I click Logo
    When I click on the element "#UboraLogo"
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I click Log in
    When I click on the element "span=Log in"
    Then I expect the title of the page "Sign in to UBORA - UBORA"

Scenario: I click Terms of Service
    When I click on the element "a=Terms of Service"
    Then I expect the title of the page "Sign up - UBORA"

Scenario: I submit valid registration form then user is logged in and full name displayed
    When I click on the element "span=Log in"
        And I sign up as "email@email.com"
        And I click on the element "span=Profile"
    Then I expect the element "h2=firstName lastName" is visible
    When I click on the element "button=Sign out"
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I sumbit empty registration form and try to create an account
    When I click on the element "button=Create an account"
    Then I expect the element "#FirstName-error=The FirstName field is required." is visible
        And I expect the element "#LastName-error=The LastName field is required." is visible
        And I expect the element "#Email-error=The Email field is required." is visible
        And I expect the element "#Password-error=The Password field is required." is visible
        And I expect the element "#ConfirmPassword-error=The Confirm password field is required." is visible
        And I expect the element "#IsAgreedToTermsOfService-error=This field is required." is visible
        And I expect the title of the page "Sign up - UBORA"