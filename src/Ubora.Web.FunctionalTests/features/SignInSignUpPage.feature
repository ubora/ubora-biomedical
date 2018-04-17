Feature: Sign in Sign up page
    As a user
    I want to click all buttons on SignInSignUp page

Background:
    Given I go to Home page
        And I clicked on the element "span=Log in"

Scenario: I am on Sign in Sign up page
    Then I expect the title of the page "Log in to UBORA - UBORA"

Scenario: Click Sign up
    When I click on the element "a=Sign up"
    Then I expect the title of the page "Sign up to UBORA - UBORA"

Scenario: Click Forgot password?
    When I click on the element "a=Forgot password?"
    Then I expect the title of the page "Forgot your password? - UBORA"
    When I click on the element "button=Submit"
    Then I expect the element "#Email-error=The Email field is required." is visible
    When I set value "test@agileworks.eu" to the element "#Email"
        And I click on the element "button=Submit"
    Then I expect the title of the page "Reset Password - UBORA"

Scenario: I click sign in without credentials
    When I click on the element "button=Log in"
    Then I expect the element "#Email-error=The Email field is required." is visible
        And I expect the element "#Password-error=The Password field is required." is visible
        And I expect the title of the page "Log in to UBORA - UBORA"