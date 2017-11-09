Feature: Sign in Sign up page
    As a user
    I want to click all buttons on SignInSignUp page

Background:
    Given I go to Home page
        And I clicked on the element "span=Log in"

Scenario: Click Logo
    When I click on the element "#UboraLogo"
    Then I expect the title of the page "Welcome - UBORA"

Scenario: Click Log in
    When I click on the element "span=Log in"
    Then I expect the title of the page "Sign in to UBORA - UBORA"

Scenario: Click Sign up
    When I click on the element "a=Sign up"
    Then I expect the title of the page "Sign up - UBORA"

Scenario: Click Forgot password?
    When I click on the element "a=Forgot password?"
    Then I expect the title of the page "Forgot your password? - UBORA"
    When I click on the element "button=Submit"
    Then I expect the element "span=The Email field is required." is visible
    When I set value "test@agileworks.eu" to the element "#Email"
        And I click on the element "button=Submit"
    Then I expect the title of the page "Reset Password - UBORA"

Scenario: I sign in without credentials
    When I click on the element "button=Sign in"
    Then I expect the element "span=The Email field is required." is visible
        And I expect the element "span=The Password field is required." is visible
        And I expect the title of the page "Sign in to UBORA - UBORA"