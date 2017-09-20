Feature: Email confirmation
    As a user 
    I want to resend email confirmation link

Background:
    Given I go to Home page
        And I signed in as user
        And I expected the element "p=We have sent an email confirmation link to test@agileworks.eu" is visible
        And I expected the title of the page "Welcome - UBORA"

Scenario: I click Resend confirmation link
    When I click on the element "a=Resend confirmation email?"
    Then I expect the title of the page "Email confirmation - UBORA"
        And I expect the element "h1=Email confirmation" is visible
        And I expect the element "p=The confirmation link has been resent to your email account." is visible
